using SQLite;
using EventGoApp.Models;

namespace EventGoApp.Services;

public class SqliteTicketRepository : ITicketRepository
{
    private readonly SQLiteAsyncConnection _db;

    public SqliteTicketRepository(SqliteService sqliteService)
    {
        _db = sqliteService.GetConnection();
    }

    public async Task AddAsync(Ticket ticket)
        => await _db.InsertAsync(ticket);

    public async Task<List<Ticket>> GetByUserAsync(Guid userId)
        => await _db.Table<Ticket>()
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.PurchasedAt)
            .ToListAsync();
}
