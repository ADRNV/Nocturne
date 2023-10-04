using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nocturne.Infrastructure.Messaging.Models;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Infrastructure.Security.EntitiesConfiguration;

namespace Nocturne.Infrastructure.Securiry
{
    public class UsersContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<UserGroup> UsersGroups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new RoleConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
