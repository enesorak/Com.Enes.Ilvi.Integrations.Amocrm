using Domain.Abstractions;

namespace Domain.Events;

public interface IEventRepository : IRepository<Event, EventId>
{
    Task<List<string>> GetAllEventsIdsAsync();
}