using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;

namespace Nocturne.Features.CurrentUser
{
    public class GetUserGroups
    {
        public record Command(Guid UserId, int Page, int PageSize) : IRequest<UserGroupsRecordSet>;

        public class Handler : IRequestHandler<Command, UserGroupsRecordSet>
        {
            private readonly UserManager<EntityUser> _userManager;

            private readonly IMapper _mapper;

            public Handler(UserManager<EntityUser> userManager, IMapper mapper)
            {
                _userManager = userManager;

                _mapper = mapper;
            }

            public async Task<UserGroupsRecordSet?> Handle(Command request, CancellationToken cancellationToken)
            {

                var allUserGroups = await _userManager.Users
                    .Include(g => g.UserGroups)
                    .AsNoTracking()
                    //.Skip((request.Page - 1) * request.PageSize)
                    //.Take(request.PageSize)
                    .Where(u => u.Id == request.UserId)
                    .Select(u => u.UserGroups)
                    .FirstOrDefaultAsync()
                    ;

                var page = allUserGroups
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize);

                var records = allUserGroups is null ? Enumerable.Empty<CoreGroup>() :
                    _mapper.Map<IEnumerable<UserGroup>, IEnumerable<CoreGroup>>(page.AsEnumerable());

                return new UserGroupsRecordSet(records, allUserGroups.Count());
            }
        }
    }
}
