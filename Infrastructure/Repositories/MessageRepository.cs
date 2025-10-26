using Domain.Messages;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MessageRepository(ApplicationDbContext context) : Repository<Message,MessageId>(context), IMessageRepository
{
    public async  Task<List<string>> GetAllIdsAsync() => 
        await Context.Set<Message>().Select(x=> x.Id.Value).ToListAsync();
}