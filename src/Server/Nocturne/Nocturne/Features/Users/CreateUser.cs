using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security.Entities;
using System.Security.Claims;

namespace Nocturne.Features.Users
{
    public class CreateUser
    {
        public record Command(CoreUser User, string Role) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly UserManager<User> _userManager;

            private readonly IPasswordHasher<User> _passwordHasher;

            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IPasswordHasher<User> passwordHasher, IMapper mapper)
            {
                _userManager = userManager;

                _passwordHasher = passwordHasher;

                _mapper = mapper;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = _mapper.Map<User>(request.User);

                user.PasswordHash = _passwordHasher.HashPassword(user, request.User.Pasword);

                var createuser = await _userManager.CreateAsync(user);

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role, request.Role),
                    new Claim(ClaimTypes.Name, request.User.UserName),
                    new Claim(ClaimTypes.Email, request.User.Login)
                };

                await _userManager.AddToRoleAsync(user, request.User.Role);
                await _userManager.AddClaimsAsync(user, claims);

                return createuser.Succeeded;
            }
        }
    }
}
