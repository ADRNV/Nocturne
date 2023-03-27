using Nocturne.Core.Repositories;
using Redis.OM.Searching;

namespace Nocturne.Infrastructure.Caching
{
    public interface IRedisCacheRepository<T> : ICacheRepository<T, IRedisCollection<T>>
    {

    }
}
