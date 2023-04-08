using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Nocturne.Core.Managers;
using Nocturne.Core.Models;
using Nocturne.Features.Messaging.Clients;
using SignalRSwaggerGen.Attributes;
using User = Nocturne.Infrastructure.Security.Entities.User;

namespace Nocturne.Features.Messaging.Hubs
{
    [SignalRHub("hubs/messages")]
    public class MessageHub : HubBase<IChatClient>
    {
        public MessageHub(IConnectionsManager connectionManager, UserManager<User> userManager, IMapper mapper) :
            base(connectionManager, userManager, mapper)
        {

        }

        public async Task<bool> SendToUser(Message message, string userName) =>
            await _mediator.Send(new SendMessage.Command(this, message, userName));

        public async Task<bool> ReciverFromUser(Message message, string from) =>
            await _mediator.Send(new ReciveMessage.Command(this, message, from));

    }
}
