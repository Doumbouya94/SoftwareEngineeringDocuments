using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Définit le contrat pour l'accès aux données d'événements, indépendamment de la source.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron de conception : Adaptateur — découple HomeViewModel de toute source de données concrète.
/// UserStories : US2.1 (affichage de la liste), US2.2 (filtrage par catégorie), US2.3 (filtrage par date et prix), US2.5 (recherche par mot-clé).
/// Épic : Découverte et recherche d'événements.
/// </remarks>
public interface IEventAdapter
{
    /// <summary>Retourne tous les événements triés par date croissante.</summary>
    Task<IReadOnlyList<Event>> GetAllAsync();

    /// <summary>Retourne les événements d'une catégorie donnée.</summary>
    Task<IReadOnlyList<Event>> GetByCategoryAsync(EventCategory category);

    /// <summary>Retourne les événements filtrés par catégorie, ville, prix et date.</summary>
    Task<IReadOnlyList<Event>> GetFilteredAsync(EventCategory? category, string? city, decimal? maxPrice, string? dateFilter, bool isFreeOnly);

    /// <summary>Récupère un événement spécifique par son Id.</summary>
    Task<Event?> GetByIdAsync(Guid id);

    /// <summary>Ajoute un nouvel événement à la source de données.</summary>
    Task AddAsync(Event newEvent);

    /// <summary>Retourne les événements créés par un organisateur donné.</summary>
    Task<List<Event>> GetByOrganizerAsync(Guid organizerId);

    /// <summary>Met à jour un événement existant.</summary>
    Task UpdateAsync(Event ev);

    /// <summary>Supprime un événement par son identifiant.</summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Recherche des événements par mot-clé sur le titre, la description et le lieu.
    /// La recherche est insensible à la casse et compatible avec les filtres de catégorie.
    /// </summary>
    Task<IReadOnlyList<Event>> SearchAsync(string query, EventCategory? category = null);
}