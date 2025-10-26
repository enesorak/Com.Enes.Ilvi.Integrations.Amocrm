using Domain.Abstractions;

namespace Domain.Tasks.TaskTypes;

public interface ITaskTypeRepository : IRepository<TaskType, TaskTypeId>
{
    Task TruncateAsync(CancellationToken cancellationToken = default);
}