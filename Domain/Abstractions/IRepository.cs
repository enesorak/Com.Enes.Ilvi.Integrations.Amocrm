using System.Linq.Expressions;

namespace Domain.Abstractions;

public interface IRepository<TEntity, in TEntityId>
{
    
    Task<TEntity?> GetByIdAsync(TEntityId id);

    void Add(TEntity entity);
    void Update(TEntity entity);
    
    void UpdateRange(List<TEntity> entities);
    void Delete(TEntity entity);
    
    
    void BulkInsert(List<TEntity> entities);
    void BulkUpdate(List<TEntity> entities);
    
 
    Task<IEnumerable<TEntity>> GetAllAsync();


    void DeleteAll(CancellationToken cancellationToken = default);
   

    Task<int> GetTotalRecords();
 
    IEnumerable<TEntity> GetPaginatedResults(int pageNumber, int pageSize);
    IQueryable<TEntity?>
        FindFilter(Expression<Func<TEntity?, bool>> filter);
    Task AddAsync(TEntity entity,
        CancellationToken cancellationToken);
    Task AddRangeAsync(List<TEntity> entities,
        CancellationToken cancellationToken);



    /*
    Task BulkInsertOrUpdateOrDeleteAsync(List<TEntity> entities,
        CancellationToken cancellationToken);
    Task BulkInsertOrUpdateAsync(List<TEntity> entities,
        CancellationToken cancellationToken);

        */
}