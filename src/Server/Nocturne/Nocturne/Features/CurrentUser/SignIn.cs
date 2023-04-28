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

        public class CommandHandler : IRequestHandler<Command, JwtAuthResult>
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

            public async Task<JwtAuthResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(request.User.Login);

                var signIn = await _signInManager.PasswordSignInAsync(user, request.User.Pasword, true, false);

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
