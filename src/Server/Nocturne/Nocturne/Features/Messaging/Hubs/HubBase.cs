using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Nocturne.Core.Managers;
using Nocturne.Core.Models;
using Nocturne.Features.Messaging.Clients;
using Nocturne.Infrastructure.Security.Entities;
using SignalRSwaggerGen.Attributes;
using User = Nocturne.Infrastructure.Security.Entities.User;

namespace Nocturne.Features.Messaging.Hubs
{
    public abstract class HubBase<TClient> : Hub<TClient> where TClient : class
    {
        protected readonly IConnectionsManager _connectionManager;

        protected readonly UserManager<User> _userManager;

        protected readonly IMapper _mapper;

        protected readonly IMediator _mediator;

        public HubBase(IConnectionsManager connectionManager, UserManager<User> userManager, IMapper mapper)
        {
            _connectionManager = connectionManager;

            _userManager = userManager;

            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;

            var user = _mapper.Map<User, Nocturne.Core.Models.User>(await _userManager.FindByNameAsync(userName));

            await _connectionManager.Connect(user, Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User.Identity.Name;

            var user = _mapper.Map<User, Nocturne.Core.Models.User>(await _userManager.FindByNameAsync(userName));

            if (!await _connectionManager.Disconect(user, Context.ConnectionId))
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
