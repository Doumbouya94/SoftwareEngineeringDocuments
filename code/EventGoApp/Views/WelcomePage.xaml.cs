using EventGoApp.Services;

namespace EventGoApp.Views;

public partial class WelcomePage : ContentPage
{
    private readonly IAuthState _authState;

    public WelcomePage(IAuthState authState)
    {
        InitializeComponent();
        _authState = authState;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("login");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("register");
    }

    private async void OnGuestClicked(object sender, EventArgs e)
    {
        _authState.SetGuest();
        await Shell.Current.GoToAsync("onboarding");
    }
}
