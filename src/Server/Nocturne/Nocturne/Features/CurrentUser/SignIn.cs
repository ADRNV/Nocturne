using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security;
using Nocturne.Models;
using System.Net;
using System.Security.Claims;
using User = Nocturne.Core.Models.User;
using UserInfrastructure = Nocturne.Infrastructure.Security.Entities.User;

namespace Nocturne.Features.CurrentUser
{
    public class SignIn
    {
        public record Command(User User) : IRequest<JwtAuthResult>;

        public class CommandHandler : IRequestHandler<Command, JwtAuthResult>
        {
            private readonly SignInManager<UserInfrastructure> _signInManager;

            private readonly PasswordHasher<UserInfrastructure> _passwordHasher;

            private readonly IJwtAuthManager _jwtAuthManager;

            public CommandHandler(SignInManager<UserInfrastructure> signInManager, PasswordHasher<UserInfrastructure> passwordHasher, IJwtAuthManager jwtAuthManager)
            {
                _signInManager = signInManager;
                _passwordHasher = passwordHasher;
                _jwtAuthManager = jwtAuthManager;
            }

            public async Task<JwtAuthResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(request.User.Login);

                var signIn = await _signInManager.PasswordSignInAsync(user, _passwordHasher.HashPassword(user, request.User.Pasword), true, false);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, request.User.Login),
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
