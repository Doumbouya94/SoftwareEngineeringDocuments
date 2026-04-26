namespace EventGoApp.Services;

/// <summary>
/// Implémentation singleton de IAuthState. Gère l'état d'authentification en mémoire.
/// </summary>
/// <remarks>
/// Auteur : Pierre
/// Patron de conception : State — gère les transitions entre LoggedOut, Guest et LoggedIn.
/// UserStories : US1.2 (connexion), US1.4 (déconnexion), US1.6 (persistance de session).
/// Épic : Authentification et gestion des utilisateurs.
/// </remarks>
public class AuthStateService : IAuthState
{
    public static readonly Guid GuestId = new Guid("00000000-0000-0000-0000-000000000001");
    /// <summary>Mode d'authentification actuel.</summary>
    public AuthMode CurrentMode { get; private set; } = AuthMode.LoggedOut;

    /// <summary>Utilisateur connecté. Null si invité ou déconnecté.</summary>
    public Models.User? CurrentUser { get; private set; }

    /// <summary>Vrai si l'utilisateur est connecté ou invité.</summary>
    public bool IsAuthenticated => CurrentMode != AuthMode.LoggedOut;

    /// <summary>Passe l'état à Invité et efface l'utilisateur courant.</summary>
    public void SetGuest()
    {
        CurrentMode = AuthMode.Guest;
        CurrentUser = null;
    }

    /// <summary>Passe l'état à Connecté et enregistre l'utilisateur.</summary>
    /// <param name="user">Utilisateur authentifié.</param>
    public void SetLoggedIn(Models.User user)
    {
        CurrentMode = AuthMode.LoggedIn;
        CurrentUser = user;
    }

    /// <summary>Passe l'état à Déconnecté et efface l'utilisateur courant.</summary>
    public void SetLoggedOut()
    {
        CurrentMode = AuthMode.LoggedOut;
        CurrentUser = null;
    }
}
