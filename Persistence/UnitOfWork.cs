using Application.Abstractions;
using Domain.Abstractions;
using Domain.Models.Users;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in _context.ChangeTracker.Entries().Where(e => e.Entity is IValidatable))
        {
            var entity = (IValidatable)entry.Entity;
           entity.Validate();
        }

        return _context.SaveChangesAsync(cancellationToken);
    }
}