using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Nocturne.Infrastructure.Caching;
using Nocturne.Infrastructure.Security.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Nocturne.Infrastructure.Security
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly JwtTokenOptions _jwtTokenConfig;
        private readonly byte[] _secret;
        private readonly UserManager<User> _usersStore;
        private readonly IRedisCacheRepository<RefreshToken> _tokensCache;

        public JwtAuthManager(JwtTokenOptions jwtTokenConfig, UserManager<User> usersStore, IRedisCacheRepository<RefreshToken> tokensCache)
        {
            _jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
            _usersStore = usersStore;
            _tokensCache = tokensCache;
        }

        public async Task RemoveExpiredRefreshTokens(DateTime now)
        {
            var expiredTokens = _tokensCache.Cache.Where(x => x.ExpireAt < now).ToList();

            foreach (var expiredToken in _tokensCache.Cache.Where(t => t.ExpireAt < now))
            {
                await _tokensCache.Delete(expiredToken);
            }
        }

        public async Task RemoveRefreshTokenByUserName(string userName)
        {
            var refreshTokens = _tokensCache.Cache.Where(x => x.UserName == userName).ToList();

            foreach (var refreshToken in refreshTokens)
            {
                await _tokensCache.Delete(refreshToken);
            }
        }

        public async Task<JwtAuthResult> GenerateTokens(User user, Claim[] claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            var jwtToken = new JwtSecurityToken(
                _jwtTokenConfig.Issuer,
                shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshToken
            {
                UserName = user.UserName,
                TokenString = GenerateRefreshTokenString(),
                ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
            };

            try
            {
                await _tokensCache.Update(refreshToken);
            }
            catch
            {
                await _tokensCache.Insert(refreshToken);
            }

            await _usersStore.SetAuthenticationTokenAsync(user, "Default", "AccessToken", jwtToken.ToString());

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<JwtAuthResult> Refresh(string refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var user = new User()
            {
                UserName = principal.Identity?.Name
            };

            var existingRefreshToken = _tokensCache.Cache
                .Where(t => t.TokenString == refreshToken)
                .FirstOrDefault();

            if (existingRefreshToken is null)
            {
                throw new SecurityTokenException("Invalid token");
            }
            if (existingRefreshToken.UserName != user.UserName || existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return await GenerateTokens(user, principal.Claims.ToArray(), now); // need to recover the original claims
        }

        public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _jwtTokenConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secret),
                        ValidAudience = _jwtTokenConfig.Audience,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
