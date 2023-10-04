using Nocturne.Core.Clients;

namespace Nocturne.Api.Client.Messaging
{
    public abstract class MessagingClientBase : IMessagingClient
    {
        public abstract Func<CoreMessage, string, bool> RecivedMessageCallback { get; set; }

        public abstract Task<bool> SendMessage(CoreMessage message, string to);

        public abstract Task Start();

        public abstract Task Stop();

        public abstract void Dispose();
    }
}
