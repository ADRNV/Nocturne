using Redis.OM.Modeling;
using System.Text.Json.Serialization;

namespace Nocturne.Infrastructure.Security
{
    [Document(Prefixes = new[] { "RefreshToken" })]
    public class RefreshToken
    {
        [RedisIdField]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("tokenString")]
        public string TokenString { get; set; }

        [JsonPropertyName("expireAt")]
        public DateTime ExpireAt { get; set; }
    }
}