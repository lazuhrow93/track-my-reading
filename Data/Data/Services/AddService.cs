using Data.CRUD.Create;
using Data.CRUD.Read;
using Database.Entites;

namespace Data.Services;

public interface IAddService
{
    Task<bool> AddBook(string title, string author, CancellationToken cancellationToken);
}

public class AddService : IAddService
{
    private readonly IBookQueries _bookQueries;
    private readonly IAuthorQueries _authorQueries;
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Author> _authorRepository;

    public AddService(IBookQueries bookQueries,
        IAuthorQueries authorQueries,
        IRepository<Book> bookRepository,
        IRepository<Author> authorRepository)
    {
        _bookQueries = bookQueries;
        _authorQueries = authorQueries;
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
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
            CreatedUtc = DateTime.UtcNow,
        };

        return await _bookRepository.Add(newBook, cancellationToken);
    }

    public async Task<bool> AddAuthor(string name, CancellationToken cancellationToken)
    {
        var existingAuthor = await _authorQueries.AuthorExists(name, cancellationToken);
        if (existingAuthor)
        {
            return false;
        }

        var newAuthor = new Author()
        {
            Name = name,
            CreatedUtc = DateTime.UtcNow,
        };

        return await _authorRepository.Add(newAuthor, cancellationToken);
    }
}
