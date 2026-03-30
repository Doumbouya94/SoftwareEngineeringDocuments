using SQLite;

namespace EventGoApp.Models;

/// <summary>
/// Représente un événement public dans l'application EventGo.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron de conception : aucun — modèle de données ORM.
/// UserStories : US2.1 (affichage de la liste), US2.4 (page de détails).
/// Épic : Découverte et recherche d'événements.
/// </remarks>
[Table("Events")]
public class Event
{
    /// <summary>Identifiant unique de l'événement.</summary>
    [PrimaryKey]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Titre de l'événement.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Description détaillée de l'événement.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Date et heure de l'événement.</summary>
    public DateTime Date { get; set; }

    /// <summary>Ville où se déroule l'événement.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Nom du lieu de l'événement.</summary>
    public string Venue { get; set; } = string.Empty;

    /// <summary>Catégorie de l'événement, stockée comme entier dans SQLite.</summary>
    [Indexed]
    public EventCategory Category { get; set; }

    /// <summary>Prix d'entrée en dollars. Valeur 0 signifie gratuit.</summary>
    public double Price { get; set; }

    /// <summary>Chemin vers l'image de l'événement. Vide si aucune image.</summary>
    public string ImageSource { get; set; } = string.Empty;

    /// <summary>Couleur de fond affichée si aucune image n'est disponible.</summary>
    public string ImagePlaceholderColor { get; set; } = "#1A237E";

    /// <summary>Indique si l'événement est mis en vedette sur la page d'accueil.</summary>
    public bool IsFeatured { get; set; }

    /// <summary>Date de création de l'enregistrement.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Date de la dernière modification de l'enregistrement.</summary>
    public DateTime UpdatedAt { get; set; }
}
