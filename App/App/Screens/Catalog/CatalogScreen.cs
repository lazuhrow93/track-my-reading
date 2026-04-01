using App.Screens.Books;
using App.Screens.Catalog.Resolvers;
using Data.CRUD.Read;
using Database.Entites;
using Spectre.Console;
namespace App.Screens.Catalog;

public interface ICatalogScreen : IScreen<CatalogScreenInput>
{
    
}

public class CatalogScreenInput : ScreenInput, IScreenInput
{
    public static CatalogScreenInput? Default => new()
    {
        ShouldClear = true,
    };
}

public class CatalogScreen : Screen<CatalogScreenInput>, ICatalogScreen
{
    private static readonly List<(string Label, Page Target)> _actions =
    [
        ("Add Book",    Page.AddBook),
    ];

    private IBookQueries _bookQueries;
    private readonly ICatalogScreenNavigator _navigator;

    public CatalogScreen(IBookQueries bookQueries, ICatalogScreenNavigator navigator)
    {
        _bookQueries = bookQueries;
        _navigator = navigator;
    }

    protected override async Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        var choice = await ShowInteractiveBookTable(cancellationToken);
        await _navigator.Navigate(choice.TargetPage, choice.ScreenInput, cancellationToken);
    }

    private async Task<CatalogScreenChoice> ShowInteractiveBookTable(CancellationToken cancellationToken)
    {
        var books = await _bookQueries.FetchAllWithAuthorAndStatus(cancellationToken);
        var choices = BuildChoices(books);

        int maxCursor = _actions.Count + books.Count - 1;
        int cursor = 0;

        //when building the live table, it has to line up perfectly with choices. 
        //TODO: improve this by basing it off of choices instead of just books
        await AnsiConsole.Live(BuildLiveTable(books, cursor))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    ctx.UpdateTarget(BuildLiveTable(books, cursor));
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.UpArrow)
                        cursor = Math.Max(0, cursor - 1);
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        if (cursor == maxCursor)
                            cursor = 0;
                        else
                            cursor++;
                    }
                    else if (key.Key == ConsoleKey.Enter)
                        break;
                    else
                        continue;
                }

                await Task.CompletedTask;
            });

        return choices[cursor];
    }

    private Table BuildLiveTable(List<Book>? books, int currentCursorIndex)
    {
        var table = new Table().AddColumns(CatalogMainTableDescriptor.Columns());

        var catalogScreenOptionIndex = 0;
        for (int i = 0; i < _actions.Count; i++)
        {
            var isCurrentlySelected = currentCursorIndex == catalogScreenOptionIndex;
            table.AddRow(
                string.Empty,
                FormatRow(_actions[i].Label, isCurrentlySelected),
                string.Empty,
                string.Empty);
            catalogScreenOptionIndex++;
        }

        table.AddRow("[grey]───[/]", "[grey]───────────────────[/]", "[grey]───────────────[/]", "[grey]──────[/]");

        if (books == null || books.Count == 0)
            return table;

        for (int i = 0; i < books.Count; i++)
        {
            var book = books[i];
            var isCurrentlySelected = currentCursorIndex == catalogScreenOptionIndex;

            table.AddRow(
                FormatRow(book.Id.ToString(), isCurrentlySelected),
                FormatRow(book.Title, isCurrentlySelected),
                FormatRow(book.Author!.Name, isCurrentlySelected),
                FormatRow(book.ReadingStatus!.State.ToString(), isCurrentlySelected));
            catalogScreenOptionIndex++;
        }

        table.Caption = new TableTitle("[grey]↑↓ navigate   Enter confirm[/]");
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

    private static List<CatalogScreenChoice> BuildChoices(List<Book> books)
    {
        var choices = new List<CatalogScreenChoice>
        {
            new CatalogScreenChoice()
            {
                TargetPage = Page.AddBook,
                ScreenInput = AddBookScreenInput.Default
            }
        };

        //choices for books
        foreach (var b in books)
        {
            choices.Add(new CatalogScreenChoice()
            {
                TargetPage = Page.BookDetails,
                ScreenInput = new BookDetailsScreenInput { BookId = b.Id }
            });
        }
        
        return choices;
    }
}
