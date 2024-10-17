using Microsoft.Extensions.DependencyInjection;

namespace Nocturne.Delpoyer.Command
{
    public interface ICommand
    {
        public void Execute(IServiceCollection serviceProvider);
    }
}
