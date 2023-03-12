using Nocturne.Core.Repositories;
using Nocturne.Infrastructure.Security;
using Redis.OM.Contracts;
using Redis.OM.Searching;

namespace Nocturne.Infrastructure.Caching
{
    public class RedisCacheRepository : ICacheRepository<RefreshToken>
    {
        private readonly IRedisConnectionProvider _redisConnectionProvider;

        private readonly IRedisConnection _redisConnection;

        private readonly IRedisCollection<RefreshToken> _refreshTokens;

        public IRedisCollection<RefreshToken> RefreshTokens
        {
            get => _refreshTokens;
        }

        public RedisCacheRepository(IRedisConnectionProvider redisConnectionProvider)
        {
            _redisConnectionProvider = redisConnectionProvider;

            _redisConnection = redisConnectionProvider.Connection;

            _refreshTokens = redisConnectionProvider.RedisCollection<RefreshToken>();
        }

        public async Task<RefreshToken?> Get(string id)
        {
            return await _refreshTokens.FindByIdAsync(id);
        }

        public async Task<string> Insert(RefreshToken entity)
        {
            return await _refreshTokens.InsertAsync(entity);
        }

        public Task Update(RefreshToken entity)
        {
            return _refreshTokens.UpdateAsync(entity);
        }

        public async Task Delete(RefreshToken refreshToken)
        {
            await _refreshTokens.DeleteAsync(refreshToken);
        }
    }
}
