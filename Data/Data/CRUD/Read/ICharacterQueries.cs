using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.CRUD.Read;

public interface ICharacterQueries : IEntityQueries<Character>
{
    Task<bool> CharacterExists(string name, int bookId, CancellationToken cancellationToken);
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
}
