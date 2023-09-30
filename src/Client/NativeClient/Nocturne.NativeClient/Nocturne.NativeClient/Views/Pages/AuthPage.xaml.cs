using Nocturne.NativeClient.ViewModels.Common;
using System.Windows.Controls;

namespace Nocturne.NativeClient.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage(IAuthViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
