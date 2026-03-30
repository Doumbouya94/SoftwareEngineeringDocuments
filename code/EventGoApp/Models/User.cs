using SQLite;

namespace EventGoApp.Models;
/// <summary>
/// Represente un utilisateur de l'application EventGo, avec des propriétés pour l'identification, 
/// les informations de connexion et les métadonnées de création et de mise à jour.
/// </summary>
/// /// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
[Table("Users")]
public class User
{
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Indexed, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Indexed, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}