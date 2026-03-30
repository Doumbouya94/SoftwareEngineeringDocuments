using EventGoApp.Services;

namespace EventGoApp.Views;

public partial class RegisterPage : ContentPage
{
    private readonly LocalAuthService _authService;
    private readonly IAuthState _authState;

    public RegisterPage(LocalAuthService authService, IAuthState authState)
    {
        InitializeComponent();
        _authService = authService;
        _authState = authState;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ErrorLabel.Text = "Veuillez remplir tous les champs obligatoires.";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (await _authService.EmailExistsAsync(EmailEntry.Text.Trim()))
        {
            ErrorLabel.Text = "Ce courriel est déjà utilisé.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var user = await _authService.RegisterAsync(
            EmailEntry.Text.Trim(),
            UsernameEntry.Text.Trim(),
            PasswordEntry.Text,
            FullNameEntry.Text?.Trim()
        );

        if (user == null)
        {
            ErrorLabel.Text = "Inscription échouée. Veuillez réessayer.";
            ErrorLabel.IsVisible = true;
            return;
        }

        _authState.SetLoggedIn(user);
        await Shell.Current.GoToAsync("onboarding");
    }

    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.GoToAsync("login");
    }
}
