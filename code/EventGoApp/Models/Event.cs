using SQLite;

namespace EventGoApp.Models;
/// <summary>
/// Represente un événement dans l'application EventGo, avec des propriétés pour les détails de l'événement, 
/// les métadonnées de création et de mise à jour, et des attributs pour la persistance dans une base de données SQLite.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
[Table("Events")]

public class Event

{

    [PrimaryKey]

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string City { get; set; } = string.Empty;

    public string Venue { get; set; } = string.Empty;

    [Indexed]

    public EventCategory Category { get; set; }  

    public double Price { get; set; } 

    public string ImageSource { get; set; } = string.Empty;

    public string ImagePlaceholderColor { get; set; } = "#1A237E";

    public bool IsFeatured { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}

