using EventGoApp.Services;

namespace EventGoApp.Views;

/// <summary>
/// Page de connexion de l'application EventGoApp, permettant aux utilisateurs de se connecter en utilisant 
/// leur adresse e-mail et mot de passe.
/// </summary>
public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService = new(new HttpClient());

    public LoginPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// M�thode appel�e lors du clic sur le bouton de connexion. 
    /// Valide les champs, tente de se connecter via le service d'authentification, 
    /// et g�re les erreurs ou la navigation en cons�quence.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        // Validation des champs de saisie
        if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ErrorLabel.Text = "Veuillez remplir tous les champs.";
            ErrorLabel.IsVisible = true;
            return;
        }
        // Appel du service d'authentification pour tenter de se connecter
        try
        {
            var result = await _authService.LoginAsync(EmailEntry.Text.Trim(), PasswordEntry.Text);

            if (result == null)
            {
                ErrorLabel.Text = "Courriel ou mot de passe invalide.";
                ErrorLabel.IsVisible = true;
                return;
            }

            // Stockage securise des tokens d'authentification
            await SecureStorage.SetAsync("auth_token", result.Token);
            await SecureStorage.SetAsync("refresh_token", result.RefreshToken);

            await Shell.Current.GoToAsync("//home");
        }
        catch (HttpRequestException)
        {
            ErrorLabel.Text = "Impossible de se connecter au serveur.";
            ErrorLabel.IsVisible = true;
        }
    }

    /// <summary>
    /// Redirige l'utilisateur vers la page d'inscription lorsqu'il clique sur le lien "Pas de compte ? S'inscrire".
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.GoToAsync("register");
    }
}
