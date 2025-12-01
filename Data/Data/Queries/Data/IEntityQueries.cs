using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.Queries.Data;

public interface IEntityQueries<TEntity>
    where TEntity : Entity
{
    Task<TEntity?> GetById(int id);
}

public abstract class EntityQueriesBase<TEntity> : IEntityQueries<TEntity>
    where TEntity : Entity
{
    protected readonly AppDbContext _context;

    protected EntityQueriesBase(AppDbContext context)
    {
        _context = context;
    }

    protected IQueryable<TEntity> Query => _context.Set<TEntity>().AsNoTracking();

    public Task<TEntity?> GetById(int id)
    {
        return Query.FirstOrDefaultAsync(e => e.Id == id);
    }
}
