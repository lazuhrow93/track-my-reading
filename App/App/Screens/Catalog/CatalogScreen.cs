using App.Screens.Books;
using App.Screens.Catalog.Resolvers;
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

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        var choice = await ShowInteractiveBookTable(cancellationToken);
        await _navigator.Navigate(choice.TargetPage, choice.ScreenInput, cancellationToken);
    }

    private async Task<CatalogScreenChoice> ShowInteractiveBookTable(CancellationToken cancellationToken)
    {
        var books = await _bookQueries.FetchAllWithAuthorAndStatus(cancellationToken);
        var choices = BuildChoices(books);

        int maxCursor = books.Count + _actions.Count - 1;
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
                        cursor = Math.Min(maxCursor, cursor + 1);
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

        if (books == null || books.Count == 0)
            return table;

        for (int i = 0; i < books.Count; i++)
        {
            var book = books[i];
            var isCurrentlySelected = currentCursorIndex == i;

            table.AddRow(
                FormatRow(book.Id.ToString(), isCurrentlySelected),
                FormatRow(book.Title, isCurrentlySelected),
                FormatRow(book.Author!.Name, isCurrentlySelected),
                FormatRow(book.ReadingStatus!.State.ToString(), isCurrentlySelected));
        }

        table.AddRow("[grey]───[/]", "[grey]───────────────────[/]", "[grey]───────────────[/]", "[grey]──────[/]");

        for (int i = 0; i < _actions.Count; i++)
        {
            var isCurrentlySelected = currentCursorIndex == books.Count + i;
            table.AddRow(
                string.Empty,
                FormatRow(_actions[i].Label, isCurrentlySelected),
                string.Empty,
                string.Empty);
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
        //choices for books
        var choices = books.Select(b => new CatalogScreenChoice()
        {
            TargetPage = Page.BookDetails,
            ScreenInput = new BookDetailsScreenInput { BookId = b.Id }
        }).ToList();

        //choices for actions
        choices.Add(new CatalogScreenChoice()
        {
            TargetPage = Page.AddBook,
            ScreenInput = AddBookScreenInput.Default
        });
        return choices;
    }
}
