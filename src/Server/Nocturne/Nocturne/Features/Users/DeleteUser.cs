using MediatR;
using Microsoft.AspNetCore.Identity;
using Nocturne.Infrastructure.Security.Entities;
using Nocturne.Models;
using System.Net;

namespace Nocturne.Features.Users
{
    public class DeleteUser
    {
        public record Command(Guid Id) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IUserStore<User> _userManager;

            public Handler(IUserStore<User> userManager)
            {
                _userManager = userManager;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString(), cancellationToken);

                if (user is not null)
                {
                    var delete = await _userManager.DeleteAsync(user, cancellationToken);

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
