using EventGoApp.Models;

namespace EventGoApp.Services;

public interface IEventAdapter
{
    Task<IReadOnlyList<Event>> GetAllAsync();
    Task<IReadOnlyList<Event>> GetByCategoryAsync(EventCategory category);
    Task<IReadOnlyList<Event>> GetFilteredAsync(EventCategory? category, string? city, decimal? maxPrice);
}