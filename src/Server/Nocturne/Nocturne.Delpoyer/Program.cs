using Microsoft.Extensions.Hosting;
using Nocturne.Delpoyer;
using Nocturne.Infrastructure.Securiry;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder
.ConfigureServices(s =>
{
    s.AddConfig($@"{Environment.CurrentDirectory}\appsettings.json")
    .AddDbFromEF<UsersContext>();
});