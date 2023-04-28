using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;
using System.Net;
using System.Security.Claims;

namespace Nocturne.Features.CurrentUser
{
    public class CreateAccount
    {
        public record Command(CoreUser User) : IRequest<JwtAuthResult>;

        public class CommandHendler : IRequestHandler<Command, JwtAuthResult>
        {
            private readonly SignInManager<User> _signInManager;

            private readonly UserManager<User> _userManager;

            private readonly IPasswordHasher<User> _passwordHasher;

            private readonly IJwtAuthManager _jwtAuthManager;

            private readonly IMapper _mapper;

            public CommandHendler(SignInManager<User> signInManager, UserManager<User> userManager,
                IPasswordHasher<User> passwordHasher, IJwtAuthManager jwtAuthManager, IMapper mapper)
            {
                _signInManager = signInManager;
                _userManager = userManager;
                _passwordHasher = passwordHasher;
                _jwtAuthManager = jwtAuthManager;
                _mapper = mapper;
            }

            public async Task<JwtAuthResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.User.Login);

                if (user is null)
                {
                    user = _mapper.Map<CoreUser, User>(request.User);

                    user.PasswordHash = _passwordHasher.HashPassword(user, request.User.Pasword);

                    await _userManager.CreateAsync(user);

                    var claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.Name, request.User.UserName),
                        new Claim(ClaimTypes.Email, request.User.Login)
                    };

                    await _signInManager.SignInAsync(user, true);

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
