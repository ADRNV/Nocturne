using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;
using System.Net;
using System.Security.Claims;

namespace Nocturne.Features.CurrentUser
{
    public class SignIn
    {
        public record Command(CoreUser CoreUser) : IRequest<JwtAuthResult>;

        public class CommandHandler : IRequestHandler<Command, JwtAuthResult>
        {
            private readonly SignInManager<User> _signInManager;

            private readonly PasswordHasher<User> _passwordHasher;

            private readonly IJwtAuthManager _jwtAuthManager;

            public CommandHandler(SignInManager<User> signInManager, PasswordHasher<User> passwordHasher, IJwtAuthManager jwtAuthManager)
            {
                _signInManager = signInManager;
                _passwordHasher = passwordHasher;
                _jwtAuthManager = jwtAuthManager;
            }

            public async Task<JwtAuthResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(request.CoreUser.Login);

                var signIn = await _signInManager.PasswordSignInAsync(user, _passwordHasher.HashPassword(user, request.CoreUser.Pasword), true, false);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, request.CoreUser.Login),
                };

                if (signIn.Succeeded)
                {
                    return await _jwtAuthManager.GenerateTokens(user, claims, DateTime.Now);
                }
                else
                {
                    throw new RestException(HttpStatusCode.Conflict);
                }
            }
        }

    }
}
