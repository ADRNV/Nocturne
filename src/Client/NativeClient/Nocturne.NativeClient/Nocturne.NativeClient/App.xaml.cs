using Nocturne.NativeClient.IoC;
using Nocturne.NativeClient.ViewModels.Common;
using System.Windows;

namespace Nocturne.NativeClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new ApiClientModule(), new ViewModelsModule());

            base.OnStartup(e);

            ComposeObjects();
        }

        private void ComposeObjects()
        {

            Current.MainWindow = IocKernel.Get<MainWindow>();
            Current.MainWindow.DataContext = IocKernel.Get<IMainViewModel>();
            Current.MainWindow.Show();
        }
    }
}
