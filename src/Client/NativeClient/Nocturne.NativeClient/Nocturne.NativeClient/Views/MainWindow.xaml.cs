using Nocturne.NativeClient.ViewModels.Common;
using System.Windows;

namespace Nocturne.NativeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
