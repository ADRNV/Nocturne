using Nocturne.Api.Client;
using Nocturne.Api.Client.Common;
using System.Windows.Input;

namespace Nocturne.NativeClient.ViewModels.Auth
{
    public class AuthVm : ApiVmBase<object>
    {
        private string _userName { get; set; }

        private string _password { get; set; }

        public string UserName
        {
            get => _userName;

            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get => _password;

            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public AuthVm(INocturneApiClientProxy apiClient) : base(apiClient)
        {
        }

        public ICommand SignInCommand => new Command(SignIn);

        private async void SignIn(object obj)
        {
            JwtAuthResult result = null;

            try
            {
                result = await _apiClient.SignInAsync(UserName, Password);
            }
            catch(Exception e)
            {
                await AppShell.Current.DisplayAlert("Ошибка", e.Message, "OK");
            }
        }
    }
}
