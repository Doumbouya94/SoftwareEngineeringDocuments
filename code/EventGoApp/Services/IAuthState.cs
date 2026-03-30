namespace EventGoApp.Services;

/// <summary>
/// Définit le contrat pour la gestion de l'état d'authentification de l'utilisateur.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : State — centralise les transitions entre les états connecté, invité et déconnecté.
/// UserStories : US1.2 (connexion), US1.4 (déconnexion), US1.6 (persistance de session).
/// Épic : Authentification et gestion des utilisateurs.
/// </remarks>
public interface IAuthState
{
    /// <summary>Mode d'authentification actuel.</summary>
    AuthMode CurrentMode { get; }

    /// <summary>Utilisateur actuellement connecté. Null si invité ou déconnecté.</summary>
    Models.User? CurrentUser { get; }

    /// <summary>Indique si l'utilisateur est connecté.</summary>
    bool IsAuthenticated { get; }

    /// <summary>Passe l'état à Invité.</summary>
    void SetGuest();

    /// <summary>Passe l'état à Connecté avec l'utilisateur fourni.</summary>
    void SetLoggedIn(Models.User user);

    /// <summary>Passe l'état à Déconnecté.</summary>
    void SetLoggedOut();
}

/// <summary>
/// Énumération des états d'authentification possibles.
/// </summary>
public enum AuthMode
{
    /// <summary>Aucun utilisateur connecté.</summary>
    LoggedOut,

    /// <summary>Utilisateur naviguant sans compte.</summary>
    Guest,

    /// <summary>Utilisateur connecté avec un compte.</summary>
    LoggedIn
}
