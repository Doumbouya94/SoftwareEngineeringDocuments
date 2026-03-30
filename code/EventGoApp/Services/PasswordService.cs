namespace EventGoApp.Services;

/// <summary>
/// Service de hachage et vérification des mots de passe avec BCrypt.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron de conception : aucun — service utilitaire.
/// UserStories : US1.1 (inscription), US1.2 (connexion).
/// Épic : Authentification et gestion des utilisateurs.
/// </remarks>
public class PasswordService
{
    /// <summary>
    /// Hache un mot de passe en clair avec BCrypt (10 rounds par défaut).
    /// </summary>
    /// <param name="plaintext">Mot de passe en clair.</param>
    /// <returns>Hash BCrypt du mot de passe.</returns>
    public string HashPassword(string plaintext) =>
        BCrypt.Net.BCrypt.HashPassword(plaintext);

    /// <summary>
    /// Vérifie si un mot de passe en clair correspond à un hash BCrypt.
    /// </summary>
    /// <param name="plaintext">Mot de passe en clair saisi par l'utilisateur.</param>
    /// <param name="hashed">Hash BCrypt stocké en base de données.</param>
    /// <returns>Vrai si le mot de passe correspond au hash.</returns>
    public bool VerifyPassword(string plaintext, string hashed) =>
        BCrypt.Net.BCrypt.Verify(plaintext, hashed);
}
