using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nocturne.Infrastructure.Securiry;

namespace Nocturne
{
    public class Startup
    {
        private IConfiguration _configuration;

        public void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("UserIdentity"));
            })
                .AddDefaultIdentity<IdentityUser>(options =>
                {

                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.SignIn.RequireConfirmedEmail = false;

                }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>();
        }
    }
}
