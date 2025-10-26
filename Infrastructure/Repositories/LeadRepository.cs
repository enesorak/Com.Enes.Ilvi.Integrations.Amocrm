using Domain.Leads;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LeadRepository(ApplicationDbContext context) : Repository<Lead,LeadId>(context), ILeadRepository
{
    public async Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Leads", cancellationToken);
    }
}