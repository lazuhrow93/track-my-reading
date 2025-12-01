using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.Queries.Data;

public interface IBookQueries : IEntityQueries<Book>
{
    Task<List<Book>> FetchAllWithAuthorAndStatus(CancellationToken cancellationToken);
}

public class BookQueries : EntityQueriesBase<Book>, IBookQueries
{
    public BookQueries(AppDbContext context) : base(context)
    {
    }

    public Task<List<Book>> FetchAllWithAuthorAndStatus(CancellationToken cancellationToken)
    {
        return Query.Include(b => b.Author)
            .Include(b => b.ReadingStatus)
            .ToListAsync(cancellationToken);
    }
}
