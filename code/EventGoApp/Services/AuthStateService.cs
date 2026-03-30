namespace EventGoApp.Services;

/// <summary>
/// Classe AuthStateService gère l'état d'authentification de l'utilisateur dans l'application.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : State Pattern — centralise la logique de gestion de l'état d'authentification et permet 
/// de basculer facilement entre les différents états (connecté, invité, déconnecté).
/// Hérite de : IAuthState — implémente l'interface pour garantir que toutes les méthodes nécessaires sont définies et utilisées de manière cohérente à travers l'application.
/// UserStories : US1.2 (connexion), US1.4 (déconnexion), US1.6 (persistance session) — gère l'état connecté / invité / déconnecté
/// Épic : "Gestion de l'authentification et de l'état utilisateur"
/// </remarks>
public class AuthStateService : IAuthState
{
    public AuthMode CurrentMode { get; private set; } = AuthMode.LoggedOut;
    public Models.User? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentMode != AuthMode.LoggedOut;

    /// <summary>
    /// Méthode pour définir l'état de l'utilisateur en tant qu'invité. 
    /// Cela signifie que l'utilisateur n'est pas connecté, mais peut accéder à certaines 
    /// fonctionnalités limitées de l'application.
    /// </summary>
    public void SetGuest()
    {
        CurrentMode = AuthMode.Guest;
        CurrentUser = null;
    }

    /// <summary>
    /// Méthode pour définir l'état de l'utilisateur en tant que connecté.
    /// </summary>
    /// <param name="user"></param>
    public void SetLoggedIn(Models.User user)
    {
        CurrentMode = AuthMode.LoggedIn;
        CurrentUser = user;
    }

    /// <summary>
    /// Méthode pour définir l'état de l'utilisateur en tant que déconnecté.
    /// </summary>
    public void SetLoggedOut()
    {
        CurrentMode = AuthMode.LoggedOut;
        CurrentUser = null;
    }
}
