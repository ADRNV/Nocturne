using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Features.Users
{
    public class GetUsers
    {
        public record Command(int Page, int PageSize) : IRequest<IEnumerable<CoreUser>>;

        public class Handler : IRequestHandler<Command, IEnumerable<CoreUser>>
        {
            private readonly UserManager<User> _userManager;

            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;

                _mapper = mapper;
            }

            public async Task<IEnumerable<CoreUser>> Handle(Command request, CancellationToken cancellationToken)
            {
               var users = await Task.Run(() => _userManager.Users
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .AsEnumerable());

                return _mapper.Map<IEnumerable<CoreUser>>(users);
            }
        }
    }
}
