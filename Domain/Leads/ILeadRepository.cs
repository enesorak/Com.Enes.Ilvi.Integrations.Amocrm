using Domain.Abstractions;

namespace Domain.Leads;

public interface ILeadRepository : IRepository<Lead, LeadId>
{
    Task TruncateAsync(CancellationToken cancellationToken = default);
}