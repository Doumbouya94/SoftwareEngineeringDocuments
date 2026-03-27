namespace EventGoAPI.DTOs;

// 3 DTOs pour les requêtes d'authentification et les réponses (immutable records)

/// <summary>
/// DTO pour la requête d'inscription d'un nouvel utilisateur. 
/// Contient les informations nécessaires pour créer un compte utilisateur,
/// </summary>
/// <param name="Email"></param>
/// <param name="Username"></param>
/// <param name="Password"></param>
/// <param name="FullName"></param>
public record RegisterRequest(
    string Email,
    string Username,
    string Password,
    string? FullName = null
);


/// <summary>
/// DTO pour la requête de connexion d'un utilisateur existant.
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record LoginRequest(
    string Email,
    string Password
);


/// <summary>
/// DTO pour la réponse d'authentification après une inscription ou une connexion réussie.
/// </summary>
/// <param name="Token"></param>
/// <param name="Expiration"></param>
/// <param name="UserId"></param>
/// <param name="Email"></param>
/// <param name="Username"></param>
/// <param name="RefreshToken"></param>
public record AuthResponse(
    string Token,
    DateTime Expiration,
    Guid UserId,
    string Email,
    string Username,
    string RefreshToken
);

/// <summary>
///  DTO pour la requête de rafraîchissement d'un token d'accès à l'aide d'un token de rafraîchissement valide.
/// </summary>
/// <param name="RefreshToken"></param>
public record RefreshTokenRequest(
    string RefreshToken
);


/// <summary>
/// DTO pour la requête de déconnexion d'un utilisateur, 
/// qui révoque le token de rafraîchissement associé pour empêcher toute utilisation future.
/// </summary>
/// <param name="RefreshToken"></param>
public record LogoutRequest(
    string RefreshToken
);


/// <summary>
/// DTO pour la requête de mot de passe oublié, qui contient l'adresse e-mail de 
/// l'utilisateur pour lequel le mot de passe doit être réinitialisé.
/// </summary>
/// <param name="Email"></param>
public record ForgotPasswordRequest(
    string Email
);


/// <summary>
/// DTO pour la requête de réinitialisation de mot de passe, qui contient l'adresse e-mail de l'utilisateur,
/// le token de réinitialisation et le nouveau mot de passe.
/// </summary>
/// <param name="Email"></param>
/// <param name="Token"></param>
/// <param name="NewPassword"></param>
public record ResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword
);