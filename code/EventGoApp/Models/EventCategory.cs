namespace EventGoApp.Models;

/// <summary>
/// Énumération des catégories d'événements disponibles dans l'application.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron de conception : aucun — simple énumération de données.
/// UserStories : US2.2 (filtrage par catégorie), US2.3 (filtrage par date et prix).
/// Épic : Découverte et recherche d'événements.
/// </remarks>
public enum EventCategory
{
    Concerts,
    Festivals,
    Sports,
    Parties,
    Food,
    Arts,
    Outdoor,
    Networking
}
