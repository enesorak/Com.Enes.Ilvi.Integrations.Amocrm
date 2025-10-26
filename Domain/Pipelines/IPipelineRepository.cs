using Domain.Abstractions;

namespace Domain.Pipelines;

public interface IPipelineRepository : IRepository<Pipeline, PipelineId>
{
    Task TruncateAsync(CancellationToken cancellationToken = default);
}