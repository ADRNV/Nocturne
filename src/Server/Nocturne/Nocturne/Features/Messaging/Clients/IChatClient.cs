using Nocturne.Core.Models;

namespace Nocturne.Features.Messaging.Clients
{
    public interface IChatClient
    {
        Task ReciveMessage(Message message);

        Task SendMessage(Message message);
    }
}
