using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;
using System.Net;

namespace Nocturne.Features.Users
{
    public class UpdateUser
    {
        public record Command(Guid Id) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());

                if (user is not null)
                {
                    var delete = await _userManager.UpdateAsync(user);

                    return delete.Succeeded;
                }
                else
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
            }
        }
    }
}
