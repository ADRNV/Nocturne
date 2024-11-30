using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;

namespace Nocturne.Features.Users
{
    public class GetUsers
    {
        public record Command(int Page, int PageSize) : IRequest<UsersRecordSet>;

        public class Handler : IRequestHandler<Command, UsersRecordSet>
        {
            private readonly UserManager<User> _userManager;

            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;

                _mapper = mapper;
            }

            public async Task<UsersRecordSet> Handle(Command request, CancellationToken cancellationToken)
            {
                var users = await Task.Run(() => _userManager.Users
                     .Skip((request.Page) * request.PageSize)
                     .Take(request.PageSize)
                     .AsEnumerable());

                var count = _userManager.Users.Count();

                var records = _mapper.Map<IEnumerable<CoreUser>>(users);

                return new UsersRecordSet(records, count);
            }
        }
    }
}
