using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Nocturne.Core.Managers;
using Nocturne.Core.Models;
using Nocturne.Features.Messaging.Clients;
using User = Nocturne.Infrastructure.Security.Entities.User;

namespace Nocturne.Features.Messaging.Hubs
{
    public class SendMessage
    {
        public record Command(HubBase<IChatClient> HubContext, Message Message, string To) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IConnectionsManager _connectionsManager;

            private readonly UserManager<User> _userManager;

            private readonly IMapper _mapper;

            public Handler(IConnectionsManager connectionsManager, UserManager<User> userManager, IMapper mapper)
            {
                _connectionsManager = connectionsManager;

                _userManager = userManager;

                _mapper = mapper;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var identityUser = await _userManager.FindByNameAsync(request.To);

                if (identityUser is not null)
                {
                    var user = _mapper.Map<User>(identityUser);

                    var userConnections = await _connectionsManager
                        .GetUserConnections(_mapper.Map<Core.Models.User>(user));

                    request.Message.From = user.UserName;

                    await request.HubContext.Clients.Clients(userConnections)
                        .SendMessage(request.Message);
                }

                return identityUser is null;
            }
        }
    }
}
