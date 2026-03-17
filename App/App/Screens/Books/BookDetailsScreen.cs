using App.Screens.Catalog;
using App.Screens.Characters;
using Data.CRUD.Read;
using Database.Entites;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spectre.Console;

namespace App.Screens.Books;

public interface IBookDetailsScreen : IScreen<BookDetailsScreenInput>
{
}

public class BookDetailsScreenInput : IScreenInput
{
    public IScreenInput? Default => null;

    public int BookId { get; set; }
}

public record BookDetailsScreenAction
{
    public Character? ChosenCharacter { get; set; }

    public Page? Redirect { get; set; }

    public BookDetailsScreenAction() { }

    public BookDetailsScreenAction(Character? chosenCharacter, Page? redirect)
    {
        ChosenCharacter = chosenCharacter;
        Redirect = redirect;
    }
}

internal class BookDetailsScreen : IBookDetailsScreen
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

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        if (input is not BookDetailsScreenInput parsedInput)
        {
            throw new ArgumentNullException();
        }

        var book = await _bookQueries.GetByIdWithAuthorAndStatus(parsedInput.BookId, cancellationToken);

        if (book == null)
        {
            throw new ArgumentNullException(nameof(Book));
        }

        var result = await SelectCharacter(parsedInput.BookId,
            book.Title,
            book.Author!.Name,
            book.ReadingStatus!.Percentage,
            cancellationToken);

        if (result.ChosenCharacter is null)
        {
            if (result.Redirect == Page.ViewCatalog)
                await _navigator.Navigate(Page.ViewCatalog, CatalogScreenInput.Default, cancellationToken);
            else
                await _navigator.Navigate(Page.AddCharacter, new AddCharacterScreenInput() { BookId = parsedInput.BookId }, cancellationToken);
            return;
        }
    }

    private async Task<BookDetailsScreenAction> SelectCharacter(int bookId, string bookTitle, string author, decimal percentageCompleted, CancellationToken cancellationToken)
    {
        var characters = await _characterQueries.ByBookId(bookId, cancellationToken);

        var index = 0;
        var charactersByIndex = characters.Select(b => KeyValuePair.Create(index++, b))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        int? cursor = 0;
        int currentSelection = 0;

        var returnVal = new BookDetailsScreenAction();

        await AnsiConsole.Live(BuildLiveTable(charactersByIndex, cursor!.Value, bookTitle, author, percentageCompleted))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    ctx.UpdateTarget(BuildLiveTable(charactersByIndex, cursor!.Value, bookTitle, author, percentageCompleted));

                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.UpArrow)
                        cursor = Math.Max(0, cursor!.Value - 1);
                    else if (key.Key == ConsoleKey.DownArrow)
                        cursor = Math.Min(characters.Count - 1, cursor!.Value + 1);
                    else if (key.Key == ConsoleKey.D1)
                    {
                        returnVal = new BookDetailsScreenAction(null, Page.AddCharacter);
                        break;
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        returnVal = new BookDetailsScreenAction(null, Page.ViewCatalog);
                        break;
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        returnVal = new BookDetailsScreenAction(charactersByIndex[currentSelection], null);
                        break;
                    }
                    else
                        continue;

                }

                await Task.CompletedTask;
            });

        return returnVal;
    }

    private Table BuildLiveTable(Dictionary<int, Character>? characters, int cursorIndex, string bookTitle, string author, decimal percentageCompleted)
    {
        var table = new Table().Title(new TableTitle(FormatTitle(bookTitle, author, percentageCompleted)))
            .AddColumns(BookDetailsMainTableDescriptor.Columns());

        if (characters == null)
            return table;

        foreach (var kvp in characters)
        {
            var isSeleted = kvp.Key;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];
            var isCurrentlySelected = cursorIndex == i;


            table.AddRow(
                FormatRow(character.Id.ToString(), isCurrentlySelected),
                FormatRow(character.Name, isCurrentlySelected),
                FormatRow(character.Description ?? string.Empty, isCurrentlySelected));
        }

        table.Caption = new TableTitle("[grey]↑↓ navigate   Space select   Enter confirm   [[1]] Add Character[/]");
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
}