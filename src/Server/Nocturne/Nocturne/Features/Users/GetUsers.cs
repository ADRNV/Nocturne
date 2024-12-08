using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Core.Filtering;
using Nocturne.Infrastructure.Filtering;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;

namespace Nocturne.Features.Users
{
    public class GetUsers
    {
        public record Command(int Page, int PageSize, string[] Search) : IRequest<UsersRecordSet>;

        public class Handler : IRequestHandler<Command, UsersRecordSet>
        {
            private readonly UserManager<User> _userManager;

            private readonly IMapper _mapper;

            private ITypedSearchMapper<User> _searchMapper;

            public Handler(UserManager<User> userManager, IMapper mapper, ITypedSearchMapper<User> searchMapper)
            {
                _userManager = userManager;

                _mapper = mapper;

                _searchMapper = searchMapper;
            }

            public async Task<UsersRecordSet> Handle(Command request, CancellationToken cancellationToken)
            {
                var criterias = _searchMapper.BuildSearch(request.Search);

                var users = await Task.Run(() => 
                     _userManager.Users
                     .ApplySearchCriteria(criterias.ToList())
                     .Skip((request.Page) * request.PageSize)
                     .Take(request.PageSize));

                var count = _userManager.Users.Count();

                var records = _mapper.Map<IEnumerable<CoreUser>>(users);

                return new UsersRecordSet(records, count);
            }
        }
    }
}
