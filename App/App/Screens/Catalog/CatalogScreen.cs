using Data.Queries.Data;
using Spectre.Console;

namespace App.Screens.Catalog;

public interface ICatalogScreen : IScreen
{
    
}

public class CatalogScreen : ICatalogScreen
{
    private IBookQueries _bookQueries;
    private readonly IServiceProvider _serviceProvider;

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
