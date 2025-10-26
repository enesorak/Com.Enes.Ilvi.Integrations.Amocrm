using Domain.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ContactRepository(ApplicationDbContext context) : Repository<Contact,ContactId>(context), IContactRepository
{
    
    
    public async Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Contacts", cancellationToken);
    }
}