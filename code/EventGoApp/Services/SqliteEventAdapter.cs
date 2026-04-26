using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

/// <summary>
/// Adaptateur qui rend SqliteService compatible avec IEventAdapter.
/// Traduit les appels de l'interface cible (IEventAdapter) en appels
/// vers la connexion SQLite (SQLiteAsyncConnection).
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// Patron : Adaptateur 
/// </remarks>

public class SqliteEventAdapter : IEventAdapter
{
    // Connexion SQLite récupérée depuis SqliteService (l'adapté)
    private readonly SQLiteAsyncConnection _db;

    /// <summary>
    /// Constructeur — reçoit le SqliteService et récupère directement
    /// la connexion active via GetConnection().
    /// </summary>
    public SqliteEventAdapter(SqliteService sqliteService)
    {
        _db = sqliteService.GetConnection();
    }

    /// <summary>
    /// Récupère tous les événements de la base de données,
    /// triés par date croissante.
    /// Adapte : GetAllAsync() → SQLite Table<Event>().ToListAsync()
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetAllAsync()
    {
        var events = await _db.Table<Event>().ToListAsync();
        return events.OrderBy(e => e.Date).ToList();
    }

    /// <summary>
    /// Récupère les événements filtrés par catégorie,
    /// triés par date croissante.
    /// Adapte : GetByCategoryAsync(category) → SQLite Where(e => e.Category == category)
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetByCategoryAsync(EventCategory category)
    {
        var events = await _db.Table<Event>()
            .Where(e => e.Category == category)
            .ToListAsync();
        return events.OrderBy(e => e.Date).ToList();
    }

    /// <summary>
    /// Récupère les événements filtrés par catégorie, ville et/ou prix maximum.
    /// Les filtres sont optionnels (nullable) et combinables.
    /// Note : le filtre category est appliqué côté SQLite,
    /// les filtres city et maxPrice sont appliqués en mémoire
    /// car SQLite-net ne supporte pas StringComparison ni le cast decimal directement.
    /// Adapte : GetFilteredAsync(...) → SQLite + filtres LINQ en mémoire
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetFilteredAsync(
        EventCategory? category, string? city, decimal? maxPrice)
    {
        // Requête de base sur la table Event
        var query = _db.Table<Event>();

        // Filtre par catégorie appliqué directement dans SQLite
        if (category.HasValue)
        {
            query = query.Where(e => e.Category == category.Value);
        }

        // Chargement en mémoire pour les filtres suivants
        var events = await query.ToListAsync();

        IEnumerable<Event> result = events;

        // Filtre par ville appliqué en mémoire
        if (!string.IsNullOrEmpty(city))
        {
            result = result.Where(e =>
                e.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        }

        // Filtre par prix maximum appliqué en mémoire
        if (maxPrice.HasValue)
        {
            result = result.Where(e => (decimal)e.Price <= maxPrice.Value);
        }

        return result.OrderBy(e => e.Date).ToList();
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _db.Table<Event>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Event newEvent)
    {
        await _db.InsertAsync(newEvent);
    }
}
