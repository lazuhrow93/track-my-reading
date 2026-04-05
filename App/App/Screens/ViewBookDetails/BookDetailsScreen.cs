using Data.CRUD.Read;
using Database.Entites;
using Spectre.Console;

namespace App.Screens.ViewBookDetails;

public interface IBookDetailsScreen : IScreen<BookDetailsInput>
{
}

internal class BookDetailsScreen : Screen<BookDetailsInput>, IBookDetailsScreen
{
    private readonly IBookDetailsScreenNavigator _navigator;
    private readonly ICharacterQueries _characterQueries;
    private readonly IBookQueries _bookQueries;

    public BookDetailsScreen(IBookDetailsScreenNavigator navigator, ICharacterQueries characterQueries, IBookQueries bookQueries)
    {
        _navigator = navigator;
        _characterQueries = characterQueries;
        _bookQueries = bookQueries;
    }

    protected override async Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        if (input is not BookDetailsInput parsedInput)
        {
            throw new ArgumentNullException();
        }

        var book = await _bookQueries.GetByIdWithAuthorAndStatus(parsedInput.BookId, cancellationToken);

        if (book == null)
        {
            throw new ArgumentNullException(nameof(Book));
        }

        var payload = await SelectCharacter(parsedInput.BookId,
            book.Title,
            book.Author!.Name,
            book.ReadingStatus!.Percentage,
            cancellationToken);

        await _navigator.Navigate(payload, cancellationToken);
    }

    private async Task<BookDetailsOnScreenAction> SelectCharacter(int bookId, string bookTitle, string author, decimal percentageCompleted, CancellationToken cancellationToken)
    {
        var characters = await _characterQueries.ByBookId(bookId, cancellationToken);
        var choices = BuildChoices(bookId, characters);

        int maxCursor = characters.Count;
        int cursor = 0;
        var returnVal = new BookDetailsOnScreenAction();

        await AnsiConsole.Live(BuildLiveTable(characters, cursor, bookTitle, author, percentageCompleted))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    ctx.UpdateTarget(BuildLiveTable(characters, cursor, bookTitle, author, percentageCompleted));

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
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        returnVal = new BookDetailsOnScreenAction(null, Page.ViewCatalog);
                        break;
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        returnVal = choices[cursor];
                        break;
                    }
                    else
                        continue;
                }

                await Task.CompletedTask;
            });

        return returnVal;
    }

    private Table BuildLiveTable(List<Character>? characters, int cursorIndex, string bookTitle, string author, decimal percentageCompleted)
    {
        var table = new Table().Title(new TableTitle(FormatTitle(bookTitle, author, percentageCompleted)))
            .AddColumns(BookDetailsMainTableDescriptor.Columns())
            .Width(Console.WindowWidth);

        table.AddRow(string.Empty, FormatRow("Add Character", cursorIndex == 0), string.Empty);

        table.AddRow("[grey]───[/]", "[grey]───────────────────[/]", "[grey]───────────────[/]");

        if (characters == null)
            return table;

        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];
            var isCurrentlySelected = cursorIndex == i + 1;

            table.AddRow(
                FormatRow(character.Id.ToString(), isCurrentlySelected),
                FormatRow(character.Name, isCurrentlySelected),
                FormatRow(character.Description ?? string.Empty, isCurrentlySelected));
        }

        table.Caption = new TableTitle("[grey]↑↓ navigate   Enter confirm[/]");
        return table;
    }

    private string FormatTitle(string title, string author, decimal percentageCompleted)
    {
        return $"[bold green]{title}[/] by [bold blue]{author}[/] - {percentageCompleted}% completed";
    }

    private string FormatRow(string rawString, bool ifSelected)
    {
        return ifSelected switch
        {
            true => $"[bold green]{rawString}[/]",
            false => rawString
        };
    }

    private static List<BookDetailsOnScreenAction> BuildChoices(int bookId, List<Character> characters)
    {
        var choices = new List<BookDetailsOnScreenAction>
        {
            BookDetailsOnScreenAction.AddCharacterForBook(bookId)
        };

        foreach (var c in characters)
        {
            choices.Add(new BookDetailsOnScreenAction(c.Id, Page.ViewCharacterDetails));
        }

        return choices;
    }
}