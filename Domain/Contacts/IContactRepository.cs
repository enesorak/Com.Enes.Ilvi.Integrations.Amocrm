using Domain.Abstractions;

namespace Domain.Contacts;

public interface IContactRepository : IRepository<Contact, ContactId>
{
    Task TruncateAsync(CancellationToken cancellationToken = default);
}
 