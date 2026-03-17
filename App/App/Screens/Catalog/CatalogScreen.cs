using App.Screens.Books;
using Data.CRUD.Read;
using Database.Entites;
using Spectre.Console;

namespace App.Screens.Catalog;

public interface ICatalogScreen : IScreen<CatalogScreenInput>
{
    
}

public class CatalogScreenInput : IScreenInput
{
    public static CatalogScreenInput? Default { get; set; }
}

public class CatalogScreen : ICatalogScreen
{
    private IBookQueries _bookQueries;
    private readonly ICatalogScreenNavigator _navigator;

    private static readonly Dictionary<string, Page> _options = new Dictionary<string, Page>
    {
        { "Add Author", Page.AddAuthor },
        { "Add Book", Page.AddBook  },
        { "Add Character", Page.AddCharacter },
    };

    public CatalogScreen(IBookQueries bookQueries, ICatalogScreenNavigator navigator)
    {
        _bookQueries = bookQueries;
        _navigator = navigator;
    }

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        var selectedBooks = await SelectBooks(cancellationToken);

        var viewBookInput = new BookDetailsScreenInput()
        {
            BookId = selectedBooks.Id,
        };

        await _navigator.Navigate(Page.BookDetails, viewBookInput, cancellationToken);
    }

    private async Task<Book> SelectBooks(CancellationToken cancellationToken)
    {
        var books = await _bookQueries.FetchAllWithAuthorAndStatus(cancellationToken);

        var index = 0;
        var booksByIndex = books.Select(b => KeyValuePair.Create(index++, b))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        int cursor = 0;
        int currentSelection = 0;

        await AnsiConsole.Live(BuildLiveTable(booksByIndex, cursor, currentSelection))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    ctx.UpdateTarget(BuildLiveTable(booksByIndex, cursor, currentSelection));
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.UpArrow)
                        cursor = Math.Max(0, cursor - 1);
                    else if (key.Key == ConsoleKey.DownArrow)
                        cursor = Math.Min(books.Count - 1, cursor + 1);
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        currentSelection = cursor;
                        break;
                    }
                    else
                        continue;
                }

                await Task.CompletedTask;
            });

        return booksByIndex[currentSelection];
    }

    private Table BuildLiveTable(Dictionary<int, Book>? books, int cursorIndex, int currentSelection)
    {
        var table = new Table().AddColumns(CatalogMainTableDescriptor.Columns());

        if (books == null)
            return table;

        foreach(var kvp in books)
        {
            var isSeleted = kvp.Key;
        }

        for (int i = 0; i < books.Count; i++)
        {
            var book = books[i];
            var isCurrentlySelected = cursorIndex == i;


            table.AddRow(
                FormatRow(book.Id.ToString(), isCurrentlySelected),
                FormatRow(book.Title, isCurrentlySelected),
                FormatRow(book.Author!.Name, isCurrentlySelected),
                FormatRow(book.ReadingStatus!.State.ToString(), isCurrentlySelected));
        }

        table.Caption = new TableTitle("[grey]↑↓ navigate   Space select   Enter confirm[/]");
        return table;
    }

    private string FormatRow(string rawString, bool ifSelected)
    {
        return ifSelected switch
        {
            true => $"[bold green]{rawString}[/]",
            false => rawString
        };
    }
}