using Application.Abstractions.Repositories;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public abstract class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
    where TEntity : class, IEntity<TEntityId>
{
    protected readonly DbContext DbContext;

    protected Repository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();
        
        if (includeFunc != null) query = includeFunc(query);

        return await query.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task<List<TEntity>> GetMultipleById(List<TEntityId> entityIds,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (includeFunc != null) query = includeFunc(query);

        return await query.Where(e => entityIds.Contains(e.Id)).ToListAsync();
    }

    public async Task<TEntity> GetById(TEntityId entityId,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (includeFunc != null) query = includeFunc(query);

        var entity = await query.FirstOrDefaultAsync(e => e.Id.Equals(entityId));

        if (entity is null) throw new ArgumentOutOfRangeException(nameof(entityId), "entityId not found in database");

        return entity;
    }

    public async Task<TEntity?> TryGetById(TEntityId entityId,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        if (includeFunc != null) query = includeFunc(query);

        return await query.FirstOrDefaultAsync(e => e.Id.Equals(entityId));
    }

    public async Task DeleteById(TEntityId entityId)
    {
        var entity = await GetById(entityId);
        DbContext.Set<TEntity>().Remove(entity);
    }

    public async Task DeleteRange(List<TEntityId> entityIds)
    {
        var entities = await GetMultipleById(entityIds);
        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    public bool BeInDatabase(TEntityId entityId)
    {
        var entity = TryGetById(entityId).Result;
        return entity != null;
    }

    public bool AllBeInDatabase(List<TEntityId> entityIds)
    {
        var idsInDatabase = GetMultipleById(entityIds).Result.Select(e => e.Id);

        return entityIds.TrueForAll(id => idsInDatabase.Contains(id));
    }

    public async Task AddRangeAsync(List<TEntity> entities)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
    }
}