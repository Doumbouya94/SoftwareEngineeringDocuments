using EventGoApp.Models;

namespace EventGoApp.Services;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket);
    Task<List<Ticket>> GetByUserAsync(Guid userId);
}
