using SQLite;

namespace EventGoApp.Models;

/// <summary>
/// Représente un utilisateur de l'application EventGo.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron de conception : aucun — modèle de données ORM.
/// UserStories : US1.1 (inscription), US1.2 (connexion), US1.5 (modification du profil).
/// Épic : Authentification et gestion des utilisateurs.
/// </remarks>
[Table("Users")]
public class User
{
    /// <summary>Identifiant unique de l'utilisateur.</summary>
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Adresse courriel unique de l'utilisateur.</summary>
    [Indexed, MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    /// <summary>Nom d'utilisateur unique.</summary>
    [Indexed, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    /// <summary>Nom complet affiché dans l'application.</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>Mot de passe haché avec BCrypt.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Ville de l'utilisateur.</summary>
    public string City { get; set; } = "Montréal";

    /// <summary>Catégories préférées (stockées sous forme de chaîne séparée par des points-virgules pour SQLite).</summary>
    public string PreferredCategoriesRaw { get; set; } = string.Empty;

    [Ignore]
    public List<EventCategory> PreferredCategories
    {
        get
        {
            if (string.IsNullOrWhiteSpace(PreferredCategoriesRaw))
                return new List<EventCategory>();

            return PreferredCategoriesRaw.Split(';')
                .Select(s => Enum.TryParse<EventCategory>(s, out var cat) ? cat : (EventCategory?)null)
                .Where(c => c.HasValue)
                .Select(c => c!.Value)
                .ToList();
        }
        set
        {
            PreferredCategoriesRaw = string.Join(";", value);
        }
    }

    /// <summary>Date de création du compte.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date de la dernière modification du compte.</summary>
    public DateTime UpdatedAt { get; set; }
}
