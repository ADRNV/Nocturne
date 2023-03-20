using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Nocturne.Core.Managers;
using Nocturne.Features.Messaging.Clients;
using Nocturne.Infrastructure.Security.Entities;
using SignalRSwaggerGen.Attributes;

namespace Nocturne.Features.Messaging.Hubs
{
    [SignalRHub("hubs/messages")]
    public class MessageHub : HubBase<IChatClient>
    {
        private readonly IConnectionsManager _connectionManager;

        private readonly UserManager<User> _userManager;

        private readonly IMapper _mapper;

        public MessageHub(IConnectionsManager connectionManager, UserManager<User> userManager, IMapper mapper)
        {
            _connectionManager = connectionManager;

            _userManager = userManager;

            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;

            var user = _mapper.Map<User,Nocturne.Core.Models.User>(await _userManager.FindByNameAsync(userName));

            await _connectionManager.Connect(user, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User.Identity.Name;

            var user = _mapper.Map<User, Nocturne.Core.Models.User>(await _userManager.FindByNameAsync(userName));
            
            if(!await _connectionManager.Disconect(user, Context.ConnectionId))
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
