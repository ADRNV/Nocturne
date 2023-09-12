using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Messaging.Models;

namespace Nocturne.Infrastructure.Security.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string? ImageUrl { get; set; }

        public List<Message?> Messages { get; set; }

        public List<UserGroup>? UserGroups { get; set; } = default;
    }
}
