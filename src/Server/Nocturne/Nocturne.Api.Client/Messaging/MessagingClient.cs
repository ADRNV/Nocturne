using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Nocturne.Core.Models;

namespace Nocturne.Api.Client.Messaging
{
    public class MessagingClient : IDisposable
    {
        private HubConnection _connection;

        private bool disposedValue;

        public Func<Message, string, bool> RecivedMessageCallback { get; set; }

        public string AccessToken { get; private set; }

        public MessagingClient(string hubUri, string accessToken, Func<Message, string, bool> recivedMessageCallback)
        {
            AccessToken = accessToken;

            RecivedMessageCallback = recivedMessageCallback;

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUri, o => {
                    o.AccessTokenProvider = () => Task.FromResult(AccessToken);
                })
                .Build();

            _connection.StartAsync();

            _connection.On("ReciveMessage", RecivedMessageCallback);
        }

        public async Task<bool> SendMessage(Message message, string to)
        {
            return await _connection.InvokeAsync<bool>("SendMessage", message, to);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _connection.DisposeAsync();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
