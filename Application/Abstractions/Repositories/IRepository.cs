namespace Application.Abstractions.Repositories;

public interface IRepository<TEntity, TEntityId>
{
    Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    Task AddAsync(TEntity entity);

    Task<List<TEntity>> GetMultipleById(List<TEntityId> entityIds,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);

    Task<TEntity> GetById(TEntityId entityId,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);

    Task<TEntity?> TryGetById(TEntityId entityId,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);

    Task DeleteById(TEntityId entityId);
    Task DeleteRange(List<TEntityId> entityIds);
    bool BeInDatabase(TEntityId entityId);
    bool AllBeInDatabase(List<TEntityId> entityIds);
    Task AddRangeAsync(List<TEntity> entities);
}