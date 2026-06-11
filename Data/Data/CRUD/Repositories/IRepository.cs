using Database;
using Database.Entites;

namespace Data.CRUD.Create;

public interface IRepository<T>
    where T : Entity
{
    Task<bool> Add(T entity, CancellationToken cancellationToken);

    Task<bool> Update(T entity, CancellationToken cancellationToken);

    Task<int> SaveChanges(CancellationToken cancellationToken);
}

public class Repository<T> : IRepository<T>
    where T : Entity
{
    private readonly AppDbContext _context;
    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Add(T entity, CancellationToken cancellationToken)
    {
        var result = await _context.Set<T>().AddAsync(entity, cancellationToken);
        var totalEntries = await SaveChanges(cancellationToken);
        return result.State == Microsoft.EntityFrameworkCore.EntityState.Added && totalEntries > 0;
    }

    public async Task<bool> Update(T entity, CancellationToken cancellationToken)
    {
        var update = _context.Set<T>().Update(entity);
        var totalEntries = await SaveChanges(cancellationToken);

        return update.State == Microsoft.EntityFrameworkCore.EntityState.Modified && totalEntries > 0;
    }

    public async Task<int> SaveChanges(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
