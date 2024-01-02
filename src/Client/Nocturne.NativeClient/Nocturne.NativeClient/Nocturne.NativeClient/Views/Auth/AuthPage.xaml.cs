using Nocturne.NativeClient.ViewModels.Auth;

namespace Nocturne.NativeClient.Views.Auth;

public partial class AuthPage : ContentPage
{
	public AuthPage(AuthVm vm)
	{
		InitializeComponent();
		
		BindingContext = vm;
	}
}