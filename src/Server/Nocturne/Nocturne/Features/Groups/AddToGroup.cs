using MediatR;
using Nocturne.Core.Managers;
using Nocturne.Core.Models;
using Nocturne.Models;

namespace Nocturne.Features.Groups
{
    public class AddToGroup
    {
        public record Command(string UserName, Group Group) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGroupManager _groupManager;

            public Handler(IGroupManager groupManager)
            {
                _groupManager = groupManager;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _groupManager.AddToGroup(request.UserName, request.Group, cancellationToken);
                }
                catch
                {
                    throw new RestException(System.Net.HttpStatusCode.NotFound);
                }
            }
        }
    }
}
