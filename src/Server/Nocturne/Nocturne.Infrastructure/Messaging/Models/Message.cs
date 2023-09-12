using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Infrastructure.Messaging.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public string Content { get; set; }
    }
}
