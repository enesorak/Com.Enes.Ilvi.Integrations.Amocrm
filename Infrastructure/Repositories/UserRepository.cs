using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : Repository<User,UserId>(context), IUserRepository
{
    public async Task TruncateAsync(CancellationToken cancellationToken = default)
    {
        await Context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Users", cancellationToken);
    }
}