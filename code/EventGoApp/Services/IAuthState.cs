namespace EventGoApp.Services;

/// <summary>
/// Interface IAuthState définit les méthodes et propriétés pour gérer l'état d'authentification
/// d'un utilisateur dans l'application.
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : State Pattern : centralise la logique de gestion de l'état d'authentification et permet 
/// de basculer facilement entre les différents états (connecté, invité, déconnecté).
/// </remarks>
public interface IAuthState
{
    /// <summary>
    /// Mode d'authentification actuel de l'utilisateur. Peut être "LoggedOut", "Guest" ou "LoggedIn".
    /// </summary>
    AuthMode CurrentMode { get; }
    /// <summary>
    /// Modèle de l'utilisateur actuellement connecté. Null si l'utilisateur est invité ou déconnecté.
    /// </summary>
    Models.User? CurrentUser { get; }
    /// <summary>
    /// Indique si l'utilisateur est authentifié (connecté).
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Méthode pour définir l'état de l'utilisateur en tant qu'invité.
    /// </summary>
    void SetGuest();
    /// <summary>
    /// Méthode pour définir l'état de l'utilisateur en tant que connecté.
    /// </summary>
    /// <param name="user">Utilisateur à connecter.</param>
    void SetLoggedIn(Models.User user);
    /// <summary>
    /// Méthode pour définir l'état de l'utilisateur en tant que déconnecté.
    /// </summary>
    void SetLoggedOut();
}


/// <summary>
/// Enumération AuthMode représente les différents états d'authentification possibles pour un 
/// utilisateur dans l'application.
/// </summary>
public enum AuthMode
{
    LoggedOut,
    Guest,
    LoggedIn
}