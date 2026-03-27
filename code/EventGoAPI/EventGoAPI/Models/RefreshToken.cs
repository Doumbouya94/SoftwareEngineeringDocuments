namespace EventGoAPI.Models;

/// <summary>
/// Représente un token de rafraîchissement utilisé pour obtenir de 
/// nouveaux tokens d'accès sans nouvelle authentification.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Obtient ou définit la chaîne de caractères représentant le token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le token a été révoqué.
    /// </summary>
    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; } 

    /// <summary>
    /// Propriété de navigation vers l'utilisateur associé à ce token de rafraîchissement.
    /// </summary>
    public ApplicationUser User { get; set; } = null!;
}
