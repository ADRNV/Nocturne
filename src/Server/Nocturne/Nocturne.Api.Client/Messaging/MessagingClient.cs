using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Nocturne.Core.Models;

namespace Nocturne.Api.Client.Messaging
{
    public class MessagingClient : MessagingClientBase
    {
        private HubConnection _connection;

        private bool disposedValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hubUri"></param>
        /// <param name="accessToken"></param>
        /// <param name="recivedMessageCallback"></param>
        public MessagingClient(string hubUri, string accessToken, Func<Message, string, bool> recivedMessageCallback)
        {
            AccessToken = accessToken;

            RecivedMessageCallback = recivedMessageCallback;

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUri, o =>
                {
                    o.AccessTokenProvider = () => Task.FromResult(AccessToken);
                })
                .Build();

            _connection.On("ReciveMessage", RecivedMessageCallback);
        }

        public override Func<Message, string, bool> RecivedMessageCallback { get; set; }

        public string AccessToken { get; private set; }

        public override async Task Start() => await _connection.StartAsync();

        public override async Task Stop() => await _connection.StopAsync();

        public override async Task<bool> SendMessage(Message message, string to)
        {
            return await _connection.InvokeAsync<bool>("SendMessage", message, to);
        }

        protected void Dispose(bool disposing)
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

        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
