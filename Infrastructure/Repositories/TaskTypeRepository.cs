using Domain.Tasks.TaskTypes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskTypeRepository(ApplicationDbContext context) : Repository<TaskType,TaskTypeId>(context), ITaskTypeRepository
{
    public async Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE TaskTypes", cancellationToken);
    }
}