using AutoMapper;
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
    public class CreateAccount
    {
        public record Command(User User) : IRequest<JwtAuthResult>;

        public class CommandHendler : IRequestHandler<Command, JwtAuthResult>
        {
            private readonly SignInManager<UserInfrastructure> _signInManager;

            private readonly UserManager<UserInfrastructure> _userManager;

            private readonly IPasswordHasher<UserInfrastructure> _passwordHasher;

            private readonly IJwtAuthManager _jwtAuthManager;

            private readonly IMapper _mapper;

            public CommandHendler(SignInManager<UserInfrastructure> signInManager, UserManager<UserInfrastructure> userManager,
                IPasswordHasher<UserInfrastructure> passwordHasher, IJwtAuthManager jwtAuthManager, IMapper mapper)
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
                    user = _mapper.Map<User, UserInfrastructure>(request.User);

                    user.PasswordHash = _passwordHasher.HashPassword(user, request.User.Pasword);

                    await _userManager.CreateAsync(user);

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Email, user.Email),
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
