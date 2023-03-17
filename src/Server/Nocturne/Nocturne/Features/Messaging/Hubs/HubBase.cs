using Microsoft.AspNetCore.SignalR;

namespace Nocturne.Features.Messaging.Hubs
{
    public abstract class HubBase<TClient> : Hub<TClient> where TClient : class
    {
    }
}
