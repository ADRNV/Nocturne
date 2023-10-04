using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nocturne.Infrastructure.Security.Entities;

namespace Nocturne.Features.CurrentUser
{
    public class GetUserGroups
    {
        public record Command(Guid UserId) : IRequest<IEnumerable<CoreGroup>>;

        public class Handler : IRequestHandler<Command, IEnumerable<CoreGroup>>
        {
            private readonly UserManager<EntityUser> _userManager;

            private readonly IMapper _mapper;

            public Handler(UserManager<EntityUser> userManager, IMapper mapper)
            {
                _userManager = userManager;

                _mapper = mapper;
            }

            public async Task<IEnumerable<CoreGroup>?> Handle(Command request, CancellationToken cancellationToken)
            {

                var groups = await _userManager.Users
                    .Include(g => g.UserGroups)
                    .AsNoTracking()
                    .Where(u => u.Id == request.UserId)
                    .Select(u => u.UserGroups)
                    .FirstOrDefaultAsync();

                return groups is null ? Enumerable.Empty<CoreGroup>() :
                    _mapper.Map<IEnumerable<UserGroup>, IEnumerable<CoreGroup>>(groups.AsEnumerable());
            }
        }
    }
}
