using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;
using System.Net;

namespace Nocturne.Features.CurrentUser
{
    public class SignIn
    {
        public record Command(string Login, string Password) : IRequest<JwtAuthResult>;

        public class CommandHandler : IRequestHandler<SignIn.Command, JwtAuthResult>
        {
            private readonly SignInManager<User> _signInManager;

            private readonly IPasswordHasher<User> _passwordHasher;

            private readonly IJwtAuthManager _jwtAuthManager;

            public CommandHandler(SignInManager<User> signInManager, IPasswordHasher<User> passwordHasher, IJwtAuthManager jwtAuthManager)
            {
                _signInManager = signInManager;
                _passwordHasher = passwordHasher;
                _jwtAuthManager = jwtAuthManager;
            }

            public async Task<JwtAuthResult> Handle(SignIn.Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(request.Login);

                var signIn = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);

                var claims = await _signInManager.UserManager.GetClaimsAsync(user);

                if (signIn.Succeeded)
                {
                    return await _jwtAuthManager.GenerateTokens(user, claims.ToArray(), DateTime.Now);
                }
                else
                {
                    throw new RestException(HttpStatusCode.Conflict);
                }
            }
        }

    }
}
