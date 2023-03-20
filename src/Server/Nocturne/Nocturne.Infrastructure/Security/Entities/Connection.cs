using Redis.OM.Modeling;

namespace Nocturne.Infrastructure.Security.Entities
{
    [Document(Prefixes = new[] { "Connection" })]
    public class Connection
    {
        [RedisIdField]
        public string Id { get; set; }

        public DateTime ConnectedAt { get; set; }

        public string ConnectionId { get; set; }

        public Guid UserId { get; set; }
    }
}
