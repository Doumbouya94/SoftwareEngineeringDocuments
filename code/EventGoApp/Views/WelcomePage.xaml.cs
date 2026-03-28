namespace EventGoApp.Views;

public partial class WelcomePage : ContentPage
{
    /// <summary>
    /// Page d'accueil de l'application EventGoApp, affich�e aux utilisateurs non authentifi�s, 
    /// offrant des options pour se connecter ou s'inscrire.
    /// </summary>
	public WelcomePage()
	{
		InitializeComponent();
	}

    /// <summary>
    /// Methode pour aller a la page de connexion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("login");
    }

    /// <summary>
    /// Méthode pour aller à la page d'inscription
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("register");
    }
}