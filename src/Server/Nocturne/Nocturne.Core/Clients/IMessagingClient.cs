using Nocturne.Core.Models;
using System.Data.Common;

namespace Nocturne.Core.Clients
{
    public interface IMessagingClient : IDisposable
    {
        Task Start();

        Task Stop();

        Task<bool> SendMessage(Message message, string to);

        Func<Message, string, bool> RecivedMessageCallback { get; set; }
    }
}
