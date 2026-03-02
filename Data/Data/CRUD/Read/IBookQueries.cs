using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.CRUD.Read;

public interface IBookQueries : IEntityQueries<Book>
{
    Task<List<Book>> FetchAllWithAuthorAndStatus(CancellationToken cancellationToken);

    Task<bool> BookExists(string title, string author, CancellationToken cancellationToken);
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

    public Task<bool> BookExists(string title, string author, CancellationToken cancellationToken)
    {
        return Query.Include(b => b.Author)
            .AnyAsync(b => b.Title == title && b.Author!.Name == author, cancellationToken);
    }
}
