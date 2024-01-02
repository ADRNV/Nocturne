namespace Nocturne.Api.Client.Common
{
    public interface INocturneApiClientProxy
    {
        Task<JwtAuthResult> SignInAsync(string login, string password);

        Task<JwtAuthResult> CreateOneAsync(User user);

    }
}
