using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Infrastructure.Security.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(ug => ug.UserGroups)
                .WithMany(ug => ug.Users);
        }
    }
}
