using Nocturne.Infrastructure.Security.Entities;
using System.Security.Claims;

namespace Nocturne.Infrastructure.Security
{
    public interface IJwtAuthManager
    {
        Task<JwtAuthResult> GenerateTokens(User user, Claim[] claims, DateTime now);

        Task<JwtAuthResult> Refresh(string refreshToken, string accessToken, DateTime now);

        Task RemoveExpiredRefreshTokens(DateTime now);

        Task RemoveRefreshTokenByUserName(string userName);
    }
}
