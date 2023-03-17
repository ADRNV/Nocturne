using Nocturne.Features.Messaging.Clients;
using SignalRSwaggerGen.Attributes;

namespace Nocturne.Features.Messaging.Hubs
{
    [SignalRHub("hubs/messages")]
    public class MessageHub : HubBase<IChatClient>
    {
    }
}
