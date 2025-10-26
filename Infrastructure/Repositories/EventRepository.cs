using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository(ApplicationDbContext context) :Repository<Event,EventId>(context), IEventRepository
{
    public async  Task<List<string>> GetAllEventsIdsAsync()
    {
       return await Context.Set<Event>().Select(x=> x.Id.Value).ToListAsync();
    }
}