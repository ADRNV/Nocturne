using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nocturne.Features.Messaging.Hubs;
using Nocturne.Infrastructure.Caching;
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
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
            });

            services.AddControllers();

            services.AddAutoMapper(c =>
            {
                c.AddProfile<IdentityUserMappingProfile>();
            }, Assembly.GetExecutingAssembly());

            services.AddSingleton<PasswordHasher<User>>();

            services.AddSingleton<IRedisCacheRepository<RefreshToken>, RedisCacheRepository<RefreshToken>>();

            services.AddScoped<IJwtAuthManager, JwtAuthManager>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

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

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Papers API", Version = "v1" });
                
                c.AddSignalRSwaggerGen();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthorization(c =>
            {
                c.AddPolicy("Administrator", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "Administrator");
                });

                c.AddPolicy("Administrator", builder =>
                {
                    builder.RequireClaim(ClaimTypes.Role, "User");
                });
            });

           services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("hubs/messages");
            });
        }
    }
}
