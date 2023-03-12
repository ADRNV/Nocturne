using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security;
using Nocturne.Models;
using System.Net;
using System.Security.Claims;
using CoreUser = Nocturne.Core.Models.User;
using SecurityUser = Nocturne.Infrastructure.Security.Entities.User;

namespace Nocturne.Features.Auth
{
    public class SignIn
    {
        public record Command(CoreUser User) : IRequest<JwtAuthResult>;

        public class CommandHandler : IRequestHandler<Command, JwtAuthResult>
        {
            private readonly SignInManager<SecurityUser> _signInManager;

            private readonly PasswordHasher<SecurityUser> _passwordHasher;

            private readonly IJwtAuthManager _jwtAuthManager;

            private readonly IMapper _mapper;

            public CommandHandler(SignInManager<SecurityUser> signInManager, PasswordHasher<SecurityUser> passwordHasher, IJwtAuthManager jwtAuthManager, IMapper mapper)
            {
                _mapper = mapper;
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
                    new Claim(ClaimTypes.Role, "User")
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
