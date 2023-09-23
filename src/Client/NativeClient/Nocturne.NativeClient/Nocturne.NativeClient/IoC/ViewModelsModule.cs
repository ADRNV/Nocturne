using Ninject.Modules;
using Nocturne.NativeClient.ViewModels;
using Nocturne.NativeClient.ViewModels.Common;
using Nocturne.NativeClient.ViewModels.MainWindow;

namespace Nocturne.NativeClient.IoC
{
    public class ViewModelsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMainViewModel>()
                .To<MainWindowViewModel>();

            Bind<IAuthViewModel>()
                .To<AuthViewModel>();
        }
    }
}
