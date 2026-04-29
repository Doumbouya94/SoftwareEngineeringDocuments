using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// CachingEventAdapter est un adaptateur d'événements qui implémente 
/// le pattern de cache pour améliorer les performances des appels d'événements.
/// Patron : Adapteur et Décorateur (Adapter et Decorator) - CachingEventAdapter implémente IEventAdapter et ajoute une couche de cache en mémoire pour les résultats d'événements.
/// Auteur : Pierre
/// </summary>

public class CachingEventAdapter : IEventAdapter
{
    private readonly IEventAdapter _inner; // Adaptateur interne pour les appels réels. (non caché, non décoré)
    private readonly Dictionary<string, object> _cache = new(); // Cache en mémoire pour les résultats d'événements.

    // Construit un adaptateur d'événements avec un adaptateur interne à mettre en cache.
    public CachingEventAdapter(IEventAdapter inner) => _inner = inner;


    /// <summary>
    /// Méthode d'accès aux événements qui vérifie d'abord le cache avant de déléguer à l'adaptateur interne.
    /// </summary>
    /// <returns>Une liste en lecture seule des événements.</returns>
    public async Task<IReadOnlyList<Event>> GetAllAsync()
    {
        const string key = "all";
        if (_cache.TryGetValue(key, out var cached))
        {
            return (IReadOnlyList<Event>)cached;
        }

        var result = await _inner.GetAllAsync();
        _cache[key] = result;
        return result;
    }

    /// <summary>
    /// Méthode d'accès aux événements filtrés par catégorie, ville, prix et date qui vérifie d'abord le cache 
    /// avant de déléguer à l'adaptateur interne.
    /// </summary>
    /// <param name="category">La catégorie d'événements à filtrer.</param>
    /// <returns>Liste d'événements filtrés par catégorie.</returns>
    public async Task<IReadOnlyList<Event>> GetByCategoryAsync(EventCategory category)
    {
        var key = $"category_{category}";
        if (_cache.TryGetValue(key, out var cached))
        {
            return (IReadOnlyList<Event>)cached;
        }

        var result = await _inner.GetByCategoryAsync(category);
        _cache[key] = result;
        return result;
    }

    /// <summary>
    /// Méthode d'accès aux événements filtrés par catégorie, ville, prix et date qui vérifie d'abord le cache 
    /// avant de déléguer à l'adaptateur interne.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="city"></param>
    /// <param name="maxPrice"></param>
    /// <param name="dateFilter"></param>
    /// <param name="isFreeOnly"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<Event>> GetFilteredAsync(
        EventCategory? category, string? city, decimal? maxPrice, string? dateFilter, bool isFreeOnly)
    {
        var key = $"filtered_{category}_{city}_{maxPrice}_{dateFilter}_{isFreeOnly}";
        if (_cache.TryGetValue(key, out var cached))
        {
            return (IReadOnlyList<Event>)cached;
        }

        var result = await _inner.GetFilteredAsync(category, city, maxPrice, dateFilter, isFreeOnly);
        _cache[key] = result;
        return result;
    }

    /// <summary>
    /// Méthode de recherche d'événements par mot-clé qui vérifie d'abord le cache avant de déléguer à l'adaptateur interne.
    /// </summary>
    /// <param name="query">Le mot-clé de recherche.</param>
    /// <param name="category">La catégorie d'événements à filtrer (optionnelle).</param>
    /// <returns>Liste d'événements correspondant à la recherche.</returns>
    public async Task<IReadOnlyList<Event>> SearchAsync(string query, EventCategory? category = null)
    {
        var key = $"search_{query}_{category}";
        if (_cache.TryGetValue(key, out var cached))
        {
            return (IReadOnlyList<Event>)cached;
        }

        var result = await _inner.SearchAsync(query, category);
        _cache[key] = result;
        return result;
    }

    /// <summary>
    /// Méthode d'accès à un événement spécifique par son identifiant qui vérifie 
    /// d'abord le cache avant de déléguer à l'adaptateur interne.
    /// </summary>
    /// <param name="id">L'identifiant de l'événement.</param>
    /// <returns>L'événement correspondant à l'identifiant, ou null s'il n'existe pas.</returns>
    public async Task<Event?> GetByIdAsync(Guid id)
    {
        var key = $"id_{id}";
        if (_cache.TryGetValue(key, out var cached))
        {
            return (Event?)cached;
        }

        var result = await _inner.GetByIdAsync(id);
        if (result is not null)
        {
            _cache[key] = result;
        }

        return result;
    }

    /// <summary>
    /// Méthode d'accès aux événements créés par un organisateur donné qui 
    /// vérifie d'abord le cache avant de déléguer à l'adaptateur interne.
    /// </summary>
    /// <param name="organizerId"></param>
    /// <returns></returns>
    public async Task<List<Event>> GetByOrganizerAsync(Guid organizerId)
    {
        var key = $"organizer_{organizerId}";
        if (_cache.TryGetValue(key, out var cached))
        {
            return (List<Event>)cached;
        }

        var result = await _inner.GetByOrganizerAsync(organizerId);
        _cache[key] = result;
        return result;
    }

    /// <summary>
    /// Méthode d'ajout d'un nouvel événement qui délègue à l'adaptateur interne et invalide le cache après l'ajout.
    /// </summary>
    /// <param name="newEvent"></param>
    /// <returns></returns>
    public async Task AddAsync(Event newEvent)
    {
        await _inner.AddAsync(newEvent);
        InvalidateCache();
    }

    /// <summary>
    /// Méthode de mise à jour d'un événement existant qui délègue à l'adaptateur interne et invalide le cache après la mise à jour.
    /// </summary>
    /// <param name="ev"></param>
    /// <returns></returns>
    public async Task UpdateAsync(Event ev) 
    {
        await _inner.UpdateAsync(ev); 
        InvalidateCache();
    }

    /// <summary>
    /// Méthode de suppression d'un événement par son identifiant qui délègue à 
    /// l'adaptateur interne et invalide le cache après la suppression.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(Guid id)
    {
        await _inner.DeleteAsync(id);
        InvalidateCache();
    }

    /// <summary>
    /// Invalide le cache en supprimant toutes les entrées. 
    /// À appeler après toute opération de mise à jour ou de suppression d'événement.
    /// </summary>
    private void InvalidateCache()
    {
        // Invalide le cache lors de la mise à jour ou de la suppression d'un événement.
        _cache.Clear();
    }
}
