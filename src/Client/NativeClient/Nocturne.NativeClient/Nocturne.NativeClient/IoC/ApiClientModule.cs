using Ninject.Modules;
using Nocturne.ApiClient;
using System.Net.Http;

namespace Nocturne.NativeClient.IoC
{
    internal class ApiClientModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Client>()
                .ToConstant(new Client("http://localhost:5196/api", new HttpClient()))
                .InSingletonScope();
        }
    }
}
