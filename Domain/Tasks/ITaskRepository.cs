using Domain.Abstractions;

namespace Domain.Tasks;

public interface ITaskRepository :IRepository<CTask, TaskId>
{
    Task TruncateAsync(CancellationToken cancellationToken = default);
}