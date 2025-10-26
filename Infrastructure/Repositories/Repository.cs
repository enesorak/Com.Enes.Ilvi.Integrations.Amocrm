using System.Linq.Expressions;
using Domain.Abstractions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<TEntity, TEntityId>(ApplicationDbContext context) : IRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : class
{
    // ReSharper disable once MemberCanBePrivate.Global
    protected readonly ApplicationDbContext Context = context;


    public void Add(TEntity entity) => Context.Add(entity);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken) =>
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken) =>
        await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

    public     void UpdateRange(List<TEntity> entities) =>
            Context.Set<TEntity>().UpdateRange(entities);


    public void Update(TEntity entity) => Context.Update(entity);

    public void Delete(TEntity entity) => Context.Remove(entity);
    public void BulkInsert(List<TEntity> entities)
    {
        Context.BulkInsert(entities);
    }

    public void DeleteAll(CancellationToken cancellationToken = default)
    {
        Context.RemoveRange( Context.Set<TEntity>());
    }
    
    public void BulkUpdate(List<TEntity> entities)
    {
        Context.BulkUpdate(entities);
    }

    public IEnumerable<TEntity> GetPaginatedResults(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity?>
        FindFilter(Expression<Func<TEntity?, bool>> filter) =>
        Context.Set<TEntity>().Where(filter);

    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await Context.Set<TEntity>().ToListAsync();

    public async Task<TEntity?> GetByIdAsync(TEntityId id)
    {
        return await Context
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: CancellationToken.None);
    }


    public async Task<int> GetTotalRecords() => await Context.Set<TEntity>().CountAsync();
}