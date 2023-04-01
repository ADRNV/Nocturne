using Microsoft.AspNetCore.Identity;

namespace Nocturne.Infrastructure.Security.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? ImageUrl { get; set; }

        public List<UserGroup>? UserGroups { get; set; } = default;
    }
}
