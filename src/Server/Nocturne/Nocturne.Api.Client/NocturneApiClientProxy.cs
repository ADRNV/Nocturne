using Nocturne.Api.Client.Common;

namespace Nocturne.Api.Client
{
    public class NocturneApiClientProxy : INocturneApiClientProxy
    {
        private Client _client;

        public NocturneApiClientProxy(Client client)
        {
            _client = client;
        }

        public Task<JwtAuthResult> CreateOneAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<JwtAuthResult> SignInAsync(string login, string password)
        {
            return await _client.SignInAsync(login, password);
        }
    }
}
