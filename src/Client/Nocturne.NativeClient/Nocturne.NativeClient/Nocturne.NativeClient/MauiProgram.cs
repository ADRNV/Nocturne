using Microsoft.Extensions.Logging;
using Nocturne.Api.Client;
using Nocturne.Api.Client.Common;
using Nocturne.NativeClient.ViewModels.Auth;
using Nocturne.NativeClient.Views.Auth;

namespace Nocturne.NativeClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddSingleton<INocturneApiClientProxy, NocturneApiClientProxy>((s) =>
            {
                var client = new Client("http://localhost:5196/", new HttpClient());

                return new NocturneApiClientProxy(client);
            });
            builder.Services.AddScoped<AuthVm>();
            builder.Services.AddTransient<AuthPage>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}