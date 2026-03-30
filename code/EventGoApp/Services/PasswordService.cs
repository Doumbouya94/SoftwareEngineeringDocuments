using System;
using System.Collections.Generic;
using System.Text;

namespace EventGoApp.Services;

/// <summary>
/// Service pour le hachage et la vérification des mots de passe.
/// </summary>
public class PasswordService
{
    /// <summary>
    /// Hache un mot de passe en clair.
    /// </summary>
    /// <param name="plaintext">Le mot de passe en clair.</param>
    /// <returns>Le mot de passe haché.</returns>
    public string HashPassword(string plaintext)
    {
        return BCrypt.Net.BCrypt.HashPassword(plaintext);
    }

    /// <summary>
    /// Vérifie si un mot de passe en clair correspond à un mot de passe haché.
    /// </summary>
    /// <param name="plaintext">Le mot de passe en clair.</param>
    /// <param name="hashed">Le mot de passe haché.</param>
    /// <returns>True si le mot de passe correspond, sinon false.</returns>
    public bool VerifyPassword(string plaintext, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(plaintext, hashed);
    }
}
