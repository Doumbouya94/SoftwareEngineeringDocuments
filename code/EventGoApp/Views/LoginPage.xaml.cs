using EventGoApp.Services;

namespace EventGoApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly LocalAuthService _authService;
    private readonly IAuthState _authState;

    public LoginPage(LocalAuthService authService, IAuthState authState)
    {
        InitializeComponent();
        _authService = authService;
        _authState = authState;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ErrorLabel.Text = "Veuillez remplir tous les champs.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var user = await _authService.LoginAsync(EmailEntry.Text.Trim(), PasswordEntry.Text);

        if (user == null)
        {
            ErrorLabel.Text = "Courriel ou mot de passe invalide.";
            ErrorLabel.IsVisible = true;
            return;
        }

        _authState.SetLoggedIn(user);
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.GoToAsync("register");
    }
}
