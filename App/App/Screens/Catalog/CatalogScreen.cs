using Data.CRUD.Read;
using Spectre.Console;

namespace App.Screens.Catalog;

public interface ICatalogScreen : IScreen
{
    
}

public class CatalogScreen : ICatalogScreen
{
    private IBookQueries _bookQueries;
    private readonly IServiceProvider _serviceProvider;

    private static readonly Dictionary<string, Page> _options = new Dictionary<string, Page>
    {
        { "Add Author", Page.AddAuthor },
        { "Add Book", Page.AddBook  },
    };

    public CatalogScreen(IBookQueries bookQueries,
        IServiceProvider serviceProvider)
    {
        _bookQueries = bookQueries;
        _serviceProvider = serviceProvider;
    }

    public async Task Show()
    {
        var table = await Build();

        AnsiConsole.Write(table);


        var choice = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
                 .Title("What would you like to do?")
                 .AddChoices(_options.Keys));

        // now we need to offer the choice for the user to add a book or an author
        // becuase currently this catalog sscreen shows empty since there are no entries

        Console.ReadLine();
    }

    private async Task<Table> Build()
    {
        var books = await _bookQueries.FetchAllWithAuthorAndStatus(CancellationToken.None);

        var table = new Table().AddColumns(CatalogMainTableDescriptor.Columns());
        
        foreach(var book in books)
        {
            table.AddRow(book.Id.ToString(), book.Title.ToString(), book.Author!.Name.ToString(), book.ReadingStatus!.State.ToString());
        }

        return table;
    }
}
