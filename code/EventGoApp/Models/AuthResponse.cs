using System;
using System.Collections.Generic;
using System.Text;

namespace EventGoApp.Models;

/// <summary>
/// Classe de modèle pour représenter la réponse d'authentification reçue de l'API d'authentification,
/// contenant les informations sur le token JWT, l'utilisateur et le token de rafraîchissement.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Token JWT reçu de l'API d'authentification, utilisé pour authentifier les requêtes ultérieures de l'utilisateur.
    /// </summary>
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// Date et heure d'expiration du token JWT.
    /// </summary>
    public DateTime Expiration { get; set; }
    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Adresse e-mail de l'utilisateur.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Nom d'utilisateur de l'utilisateur.
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// Token de rafraîchissement utilisé pour obtenir un nouveau token JWT lorsque l'ancien expire.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
