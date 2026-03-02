using Data.Queries.Data;
using Database.Entites;

namespace Data.Services;

public interface IAddService
{
    Task<bool> AddBook(string title, string author);
}

public class AddService : IAddService
{
    private readonly IBookQueries _bookQueries;
    private readonly IAuthorQueries _authorQueries;

    public AddService(IBookQueries bookQueries, IAuthorQueries authorQueries)
    {
        _bookQueries = bookQueries;
        _authorQueries = authorQueries;
    }

    public async Task<bool> AddBook(string title, string author, CancellationToken cancellationToken)
    {
        var existingBook = await _bookQueries.BookExists(title, author, cancellationToken);

        if (existingBook)
        {
            return false;
        }

        var existingAuthor = await _authorQueries.GetAuthorByName(author, cancellationToken);

        if (existingAuthor == null)
        {
            return false;
        }

        var newBook = new Book()
        {
            Title = title,
            AuthorId = existingAuthor.Id,
        };

        //need to set up the ADD functionalty for books. REally anytthing in general. we dont have a way to add to the database.
    }

}
