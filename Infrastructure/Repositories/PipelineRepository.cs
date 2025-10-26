using Domain.Pipelines;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PipelineRepository (ApplicationDbContext context) : Repository<Pipeline,PipelineId>(context), IPipelineRepository
{
    public async Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Pipelines", cancellationToken);
    }
}