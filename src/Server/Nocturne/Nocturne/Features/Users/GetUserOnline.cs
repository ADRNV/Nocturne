using MediatR;
using Nocturne.Core.Managers;

namespace Nocturne.Features.Users
{
    public class GetUserOnline
    {
        public record Command(CoreUser User) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IConnectionsManager _connectionsManager;

            public Handler(IConnectionsManager connectionsManager)
            {
                _connectionsManager = connectionsManager;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken) =>
                await _connectionsManager.IsConnectedUser(request.User);
            
        }
    }
}
