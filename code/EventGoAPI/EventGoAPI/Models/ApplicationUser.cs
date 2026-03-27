using Microsoft.AspNetCore.Identity;

namespace EventGoAPI.Models;

/// <summary>
/// Représente un utilisateur de l'application, étendant les fonctionnalités de base fournies par IdentityUser.
/// </summary>
/// <remarks>
/// Les propriétés supplémentaires de IdentityUser incluent : 
/// Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, 
/// ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled et 
/// AccessFailedCount.
/// </remarks>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Obtient ou définit le nom complet de l'utilisateur.
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// Obtient ou définit l'âge de l'utilisateur.
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    /// Définit ou obtient l'URL de l'image de profil de l'utilisateur.
    /// </summary>
    public string? ProfileImage { get; set; }
    public bool IsGuest { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
