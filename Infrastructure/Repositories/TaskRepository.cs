using Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskRepository(ApplicationDbContext context) :Repository<CTask,TaskId>(context), ITaskRepository
{
    public async Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Tasks", cancellationToken);

    }
}