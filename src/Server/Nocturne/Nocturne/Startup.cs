using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nocturne.Core.Mails;
using Nocturne.Core.Managers;
using Nocturne.Core.Repositories;
using Nocturne.Features.Messaging.Hubs;
using Nocturne.Features.Validation;
using Nocturne.Infrastructure.Caching;
using Nocturne.Infrastructure.Groups;
using Nocturne.Infrastructure.MailSending;
using Nocturne.Infrastructure.MailSending.Options;
using Nocturne.Infrastructure.Messaging;
using Nocturne.Infrastructure.Messaging.Models;
using Nocturne.Infrastructure.Messaging.Storage;
using Nocturne.Infrastructure.Securiry;
using Nocturne.Infrastructure.Security;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Infrastructure.Security.MappersConfiguration;
using Nocturne.Middlewares;
using Redis.OM;
using Redis.OM.Contracts;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Nocturne
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("IdentityDbConnection"));
            })
                .AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.SignIn.RequireConfirmedEmail = false;
                }).AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UsersContext>();

            var redisConnectionProvider = new RedisConnectionProvider(_configuration.GetConnectionString("RedisConnection")!);

            services.AddSingleton<IRedisConnectionProvider>(redisConnectionProvider);

            var jwtTokenOptions = _configuration.GetSection("jwtTokenOptions")
                .Get<JwtTokenOptions>();

            services.AddSingleton(jwtTokenOptions);

            services.AddSingleton(s =>
            {
                var options = new MailSenderOptions
                {
                    SenderName = _configuration["mailSenderOptions:sender_name"],
                    SenderEmail = _configuration["mailSenderOptions:sender_email"],
                    HostUsername = _configuration["mailSenderOptions:host_username"],
                    HostPort = 587,//Not reads from config
                    HostPassword = _configuration["mailSenderOptions:host_password"],
                    HostAddress = _configuration["mailSenderOptions:host_adress"]

                };

                return options;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddMicrosoftAccount(options => {
                    options.ClientId = _configuration["MSAuth:AppId"];
                    options.ClientSecret = _configuration["MSAuth:UserSecret"];
                 })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtTokenOptions.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenOptions.Secret)),
                        ValidAudience = jwtTokenOptions.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(60)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs/chat")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers();

            services.AddAutoMapper(c =>
            {
                c.AddProfile<IdentityUserMappingProfile>();
            }, Assembly.GetExecutingAssembly());

            services.AddScoped<IMailSender, MailSender>();

            services.AddSingleton<PasswordHasher<User>>();

            services.AddScoped<IMessagesRepository<Message>, MessagesRepository>();

            services.AddSingleton<IRedisCacheRepository<RefreshToken>, RedisCacheRepository<RefreshToken>>();

            services.AddSingleton<IRedisCacheRepository<Connection>, RedisCacheRepository<Connection>>();

            services.AddScoped<IConnectionsManager, ConnectionsManager<User>>();

            services.AddScoped<IGroupManager, GroupsManager>();

            services.AddScoped<IJwtAuthManager, JwtAuthManager>();

            services.AddMediatR(cfg =>
            {
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
                cfg.RegisterServicesFromAssemblyContaining<Startup>();
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SupportNonNullableReferenceTypes();

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nocturne API", Version = "v1" });

                c.AddSignalRSwaggerGen();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthorization(c =>
            {
                c.AddPolicy(AuthorizeConstants.Policies.Administrator, builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, AuthorizeConstants.Roles.Administrator);
                });

                c.AddPolicy(AuthorizeConstants.Policies.User, builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, AuthorizeConstants.Roles.User);
                });
            });

            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
    
            app.UseRouting();

            using (var scope =
                        app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<UsersContext>())
                context.Database.EnsureCreated();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath}";
                options.GetLevel = (httpContext, elapsed, ex) => Serilog.Events.LogEventLevel.Debug;
            });

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("hubs/messages");
            });
        }
    }
}
