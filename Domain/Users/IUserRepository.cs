using Domain.Abstractions;

namespace Domain.Users;

public interface IUserRepository : IRepository<User, UserId>
{
    Task TruncateAsync(CancellationToken cancellationToken = default);

}