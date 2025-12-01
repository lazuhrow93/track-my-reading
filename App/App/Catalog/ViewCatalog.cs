using Data.Queries.Data;
using Database.Entites;
using Spectre.Console;

namespace App.Catalog;

public class ViewCatalog
{
    private IBookQueries _bookQueries;
    
    public ViewCatalog(IBookQueries bookQueries)
    {
        _bookQueries = bookQueries;
    }

    public async Task Show()
    {
        var table = await Build();

        AnsiConsole.Write(table);
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
