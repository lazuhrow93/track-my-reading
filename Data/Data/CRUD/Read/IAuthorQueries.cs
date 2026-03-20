using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.CRUD.Read;

public interface IAuthorQueries : IEntityQueries<Author>
{
    Task<bool> AuthorExists(string name, CancellationToken cancellationToken);

    Task<Author?> GetAuthorByName(string name, CancellationToken cancellationToken);

    Task<List<Author>> FetchAll(CancellationToken cancellationToken);
}

public class AuthorQueries : EntityQueriesBase<Author>, IAuthorQueries
{
    public AuthorQueries(AppDbContext context) : base(context)
    {
    }

    public Task<bool> AuthorExists(string name, CancellationToken cancellationToken)
    {
        return Query.AnyAsync(a => a.Name == name, cancellationToken);
    }

    public Task<Author?> GetAuthorByName(string name, CancellationToken cancellationToken)
    {
        return Query.FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
    }

    public Task<List<Author>> FetchAll(CancellationToken cancellationToken)
    {
        return Query.ToListAsync(cancellationToken);
    }
}
