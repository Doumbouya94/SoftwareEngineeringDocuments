using EventGoApp.Services;

namespace EventGoApp.Views;

/// <summary>
/// Classe de la page d'inscription de l'application EventGoApp, permettant aux utilisateurs de 
/// cr�er un compte en fournissant leur adresse e-mail, nom d'utilisateur, mot de passe et nom complet (optionnel).
/// </summary>
public partial class RegisterPage : ContentPage
{
	private readonly AuthService _authService = new(new HttpClient());

	public RegisterPage()
	{
		InitializeComponent();
	}

    /// <summary>
    /// M�thode pour g�rer l'inscription de l'utilisateur lorsque le bouton "S'inscrire" est cliqu�.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;


        // Validation de base pour s'assurer que les champs obligatoires ne sont pas vides
        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ErrorLabel.Text = "Veuillez remplir tous les champs obligatoires.";
            ErrorLabel.IsVisible = true;
            return;
        }

        // Appel du service d'authentification pour enregistrer l'utilisateur
        try
        {
            var result = await _authService.RegisterAsync(
                EmailEntry.Text.Trim(),
                UsernameEntry.Text.Trim(),
                PasswordEntry.Text,
                FullNameEntry.Text?.Trim()
            );

            // Si l'inscription echoue, afficher un message d'erreur
            if (result == null)
            {
                ErrorLabel.Text = "Inscription echouee. Veuillez reessayer.";
                ErrorLabel.IsVisible = true;
                return;
            }

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
    /// M�thode pour aller � la page de connexion lorsque l'utilisateur clique sur le lien "D�j� un compte ? Se connecter".
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnLoginTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
        await Shell.Current.GoToAsync("login");
    }
}