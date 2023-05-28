using Nocturne.Core.Clients;
using Nocturne.Core.Models;

namespace Nocturne.Api.Client.Messaging
{
    public abstract class MessagingClientBase : IMessagingClient
    {
        public abstract Func<Message, string, bool> RecivedMessageCallback { get; set; }
        
        public abstract Task<bool> SendMessage(Message message, string to);
        
        public abstract Task Start();
        
        public abstract Task Stop();

        public abstract void Dispose();
    }
}
