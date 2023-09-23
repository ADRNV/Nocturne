using Nocturne.NativeClient.ViewModels.Common;
using System.Windows.Controls;

namespace Nocturne.NativeClient.ViewModels.MainWindow
{
    public class MainWindowViewModel : ViewModelBase, IMainViewModel
    {
        private Page _currentPage;

        public MainWindowViewModel() { }

        public Page CurrentPage
        {
            get => _currentPage;

            set
            {
                _currentPage = value;
                RaisePropertyChanged(nameof(CurrentPage));
            }
        }
    }
}
