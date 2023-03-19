using Nocturne.Core.Repositories;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Nocturne.Infrastructure.Caching
{
    public class RedisCacheRepository<T> : ICacheRepository<T> where T : notnull
    {
        private readonly IRedisConnectionProvider _redisConnectionProvider;

        private readonly IRedisCollection<T> _cache;

        public IRedisCollection<T> Cache
        {
            get => _cache;
        }

        public RedisCacheRepository(IRedisConnectionProvider redisConnectionProvider)
        {
            _redisConnectionProvider = redisConnectionProvider;

            _cache = redisConnectionProvider.RedisCollection<T>();
        }

        public async Task<T?> Get(string id)
        {
            return await _cache.FindByIdAsync(id);
        }

        public async Task<string> Insert(T entity)
        {
            return await _cache.InsertAsync(entity);
        }

        public Task Update(T entity)
        {
            return _cache.UpdateAsync(entity);
        }

        public async Task Delete(T entity)
        {
            await _cache.DeleteAsync(entity);
        }
    }
}
