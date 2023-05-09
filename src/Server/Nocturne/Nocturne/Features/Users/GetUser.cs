using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;
using System.Net;

namespace Nocturne.Features.Users
{
    public class GetUser
    {
        public record Command(string UserName) : IRequest<CoreUser>;

        public class Handler : IRequestHandler<Command, CoreUser>
        {
            private readonly UserManager<User> _userManager;

            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;

                _mapper = mapper;
            }

            public async Task<CoreUser> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user is not null)
                {
                    return _mapper.Map<CoreUser>(user);
                }
                else
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
            }
        }
    }
}
