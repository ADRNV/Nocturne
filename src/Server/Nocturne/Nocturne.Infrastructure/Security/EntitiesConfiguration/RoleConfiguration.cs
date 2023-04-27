using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nocturne.Infrastructure.Security.EntitiesConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
                new IdentityRole<Guid>("User") 
                { 
                    Id = Guid.NewGuid(), 
                    NormalizedName = "USER"
                },
                new IdentityRole<Guid>("Administrator")
                { 
                    Id = Guid.NewGuid(),
                    NormalizedName = "ADMINISTRATOR"
                
                });
        }
    }
}
