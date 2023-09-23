using MvvmCross.Commands;
using Nocturne.ApiClient;
using Nocturne.NativeClient.ViewModels.Common;

namespace Nocturne.NativeClient.ViewModels
{
    public class AuthViewModel : ViewModelBase, IAuthViewModel
    {
        private string _login;

        private string _password;

        private Client _client;

        public AuthViewModel(Client client)
        {
            _client = client;
        }

        public string Login
        {
            get => _login;

            set
            {
                _login = value;
                RaisePropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get => _password;

            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public IMvxCommand AuthCommand => new MvxCommand(Auth);

        public void Auth()
        {
            _client.SignInAsync(new User { Login = Login, Pasword = Password });
        }
    }
}
