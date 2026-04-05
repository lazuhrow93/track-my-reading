using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.CRUD.Read;

public interface ICharacterQueries : IEntityQueries<Character>
{
    Task<bool> CharacterExists(string name, int bookId, CancellationToken cancellationToken);

    Task<List<Character>> ByBookId(int bookId, CancellationToken cancellationToken);

    Task<Character?> GetByIdWithTraitsAndBook(int id, CancellationToken cancellationToken);
}

public class CharacterQueries : EntityQueriesBase<Character>, ICharacterQueries
{
    public CharacterQueries(AppDbContext context) : base(context)
    {
    }

    public Task<bool> CharacterExists(string name, int bookId, CancellationToken cancellationToken)
    {
        return Query.AnyAsync(c => c.Name == name && c.BookId == bookId, cancellationToken);
    }

    public Task<List<Character>> ByBookId(int bookId, CancellationToken cancellationToken)
    {
        return Query.Where(c => c.BookId == bookId)
            .ToListAsync(cancellationToken);
    }

    public Task<Character?> GetByIdWithTraitsAndBook(int id, CancellationToken cancellationToken)
    {
        return Query
            .Include(c => c.Traits)
            .Include(c => c.Book)
            .Include(c => c.Notes.OrderByDescending(n => n.CreatedUtc))
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}
