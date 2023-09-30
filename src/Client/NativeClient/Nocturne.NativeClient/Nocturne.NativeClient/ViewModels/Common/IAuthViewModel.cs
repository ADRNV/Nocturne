using MvvmCross.Commands;

namespace Nocturne.NativeClient.ViewModels.Common
{
    public interface IAuthViewModel
    {
        string Login { get; set; }

        string Password { get; set; }

        IMvxCommand AuthCommand { get; }
    }
}
