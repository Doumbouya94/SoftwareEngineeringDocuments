using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventGoApp.Models;
using EventGoApp.Services;
using SQLite;

namespace EventGoApp.Services;

/// <summary>
/// Implémentation SQLite du repository des favoris.
/// Persiste les favoris localement dans la base de données SQLite.
/// </summary>
/// <remarks>
/// Auteur : Aboubacar Sidiki Doumbouya
/// </remarks>
public class SqliteFavoriteRepository : IFavoriteRepository
{
    private readonly SQLiteAsyncConnection _db;

    public SqliteFavoriteRepository(SqliteService sqliteService)
    {
        _db = sqliteService.GetConnection();
    }

    /// <summary>Ajoute un événement aux favoris.</summary>
    public async Task AddAsync(Event @event)
    {
        var favorite = new Favorite
        {
            EventId = @event.Id,
            AddedAt = DateTime.UtcNow
        };
        await _db.InsertAsync(favorite);
    }

    /// <summary>Retire un événement des favoris par son identifiant.</summary>
    public async Task RemoveAsync(Guid eventId)
    {
        var favorite = await _db.Table<Favorite>()
            .Where(f => f.EventId == eventId)
            .FirstOrDefaultAsync();

        if (favorite != null)
            await _db.DeleteAsync(favorite);
    }

    /// <summary>Retourne tous les événements favoris.</summary>
    public async Task<IReadOnlyList<Event>> GetAllAsync()
    {
        var favorites = await _db.Table<Favorite>().ToListAsync();
        var eventIds = favorites.Select(f => f.EventId).ToHashSet();
        var events = await _db.Table<Event>().ToListAsync();
        return events.Where(e => eventIds.Contains(e.Id)).ToList();
    }

    /// <summary>Vérifie si un événement est déjà dans les favoris.</summary>
    public async Task<bool> IsFavoriteAsync(Guid eventId)
    {
        var count = await _db.Table<Favorite>()
            .Where(f => f.EventId == eventId)
            .CountAsync();
        return count > 0;
    }
}