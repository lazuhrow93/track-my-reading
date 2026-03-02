using Database;
using Database.Entites;

namespace Data.CRUD.Create;

public interface IRepository<T>
    where T : Entity
{
    Task<bool> Add(T entity, CancellationToken cancellationToken);
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
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
