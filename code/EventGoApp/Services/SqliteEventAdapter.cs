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
    /// Récupère les événements filtrés par catégorie, ville, prix maximum, date et gratuité.
    /// </summary>
    public async Task<IReadOnlyList<Event>> GetFilteredAsync(
        EventCategory? category, string? city, decimal? maxPrice, string? dateFilter, bool isFreeOnly)
    {
        // Requête de base sur la table Event
        var query = _db.Table<Event>();

        // Filtre par catégorie appliqué directement dans SQLite
        if (category.HasValue)
        {
            query = query.Where(e => e.Category == category.Value);
        }

        // Chargement en mémoire pour les filtres complexes
        var events = await query.ToListAsync();
        IEnumerable<Event> result = events;

        // Filtre par ville
        if (!string.IsNullOrEmpty(city))
        {
            result = result.Where(e =>
                e.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        }

        // Filtre par prix maximum
        if (maxPrice.HasValue)
        {
            result = result.Where(e => (decimal)e.Price <= maxPrice.Value);
        }

        // Filtre Gratuit uniquement
        if (isFreeOnly)
        {
            result = result.Where(e => e.Price == 0);
        }

        // Filtre par date
        if (!string.IsNullOrEmpty(dateFilter) && dateFilter != "Toutes les dates")
        {
            var today = DateTime.Today;
            result = dateFilter switch
            {
                "Aujourd'hui" => result.Where(e => e.Date.Date == today),
                "Demain" => result.Where(e => e.Date.Date == today.AddDays(1)),
                "Ce week-end" => result.Where(e => {
                    int daysToSaturday = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
                    var saturday = today.AddDays(daysToSaturday);
                    var sunday = saturday.AddDays(1);
                    return e.Date.Date == saturday || e.Date.Date == sunday;
                }),
                "Cette semaine" => result.Where(e => {
                    int daysToSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
                    var nextSunday = today.AddDays(daysToSunday);
                    return e.Date.Date >= today && e.Date.Date <= nextSunday;
                }),
                "Ce mois-ci" => result.Where(e => e.Date.Month == today.Month && e.Date.Year == today.Year),
                _ => result
            };
        }

        return result.OrderBy(e => e.Date).ToList();
    }

    /// <summary>
    /// Recherche des événements par mot-clé sur le titre, la description et le lieu.
    /// La recherche est insensible à la casse et compatible avec les filtres de catégorie.
    /// Adapte : SearchAsync(...) → SQLite + filtres LINQ en mémoire
    /// </summary>
    public async Task<IReadOnlyList<Event>> SearchAsync(string query, EventCategory? category = null)
    {
        // Charger tous les événements (ou filtrés par catégorie) en mémoire
        var events = category.HasValue
            ? await _db.Table<Event>().Where(e => e.Category == category.Value).ToListAsync()
            : await _db.Table<Event>().ToListAsync();

        // Si la recherche est vide, retourner tous les événements
        if (string.IsNullOrWhiteSpace(query))
        {
            return events.OrderBy(e => e.Date).ToList();
        }

        // Recherche insensible à la casse sur titre, description et lieu
        var lower = query.ToLowerInvariant();
        var result = events.Where(e =>
            e.Title.ToLowerInvariant().Contains(lower) ||
            e.Description.ToLowerInvariant().Contains(lower) ||
            e.Venue.ToLowerInvariant().Contains(lower));

        return result.OrderBy(e => e.Date).ToList();
    }

    public async Task<Event?> GetByIdAsync(Guid id)
        => await _db.Table<Event>().FirstOrDefaultAsync(e => e.Id == id);

    public async Task AddAsync(Event newEvent)
        => await _db.InsertAsync(newEvent);

    public async Task<List<Event>> GetByOrganizerAsync(Guid organizerId)
        => await _db.Table<Event>()
            .Where(e => e.OrganizerId == organizerId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

    public async Task UpdateAsync(Event ev)
    {
        ev.UpdatedAt = DateTime.UtcNow;
        await _db.UpdateAsync(ev);
    }

    public async Task DeleteAsync(Guid id)
        => await _db.DeleteAsync<Event>(id);
}