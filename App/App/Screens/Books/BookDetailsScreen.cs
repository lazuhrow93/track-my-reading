using App.Screens.Catalog;
using Data.CRUD.Read;
using Database.Entites;
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

internal class BookDetailsScreen : IBookDetailsScreen
{
    private readonly IBookDetailsScreenNavigator _navigator;
    private readonly ICharacterQueries _characterQueries;

    public BookDetailsScreen(IBookDetailsScreenNavigator navigator, ICharacterQueries characterQueries)
    {
        _navigator = navigator;
        _characterQueries = characterQueries;
    }

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        if (input is not BookDetailsScreenInput parsedInput)
        {
            throw new ArgumentNullException();
        }

        var character = await SelectCharacter(parsedInput.BookId, cancellationToken);
    }
    private async Task<Character> SelectCharacter(int bookId, CancellationToken cancellationToken)
    {
        var characters = await _characterQueries.ByBookId(bookId, cancellationToken);

        var index = 0;
        var charactersByIndex = characters.Select(b => KeyValuePair.Create(index++, b))
            .ToDictionary(kv => kv.Key, kv => kv.Value);
        int cursor = 0;
        int currentSelection = 0;

        await AnsiConsole.Live(BuildLiveTable(charactersByIndex, cursor, currentSelection))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.UpArrow)
                        cursor = Math.Max(0, cursor - 1);
                    else if (key.Key == ConsoleKey.DownArrow)
                        cursor = Math.Min(characters.Count - 1, cursor + 1);
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        currentSelection = cursor;
                        break;
                    }
                    else
                        continue;

                    ctx.UpdateTarget(BuildLiveTable(charactersByIndex, cursor, currentSelection));
                }

                await Task.CompletedTask;
            });

        return charactersByIndex[currentSelection];
    }

    private Table BuildLiveTable(Dictionary<int, Character>? characters, int cursorIndex, int currentSelection)
    {
        var table = new Table().AddColumns(BookDetailsMainTableDescriptor.Columns());

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