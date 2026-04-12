using Data.CRUD.Read;
using Database.Entites;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace App.Screens.SearchForCharacter;

public interface ISearchForCharacterScreen : IScreen<SearchForCharacterScreenInput>
{
}

public class SearchForCharacterScreenInput : ScreenInput, IScreenInput
{
    public static SearchForCharacterScreenInput Default => new() { ShouldClear = true };
}

public class SearchForCharacterScreen : Screen<SearchForCharacterScreenInput>, ISearchForCharacterScreen
{
    private readonly IBookQueries _bookQueries;
    private readonly ICharacterQueries _characterQueries;
    private readonly ITraitQuieries _traitQueries;
    private readonly ISearchForCharacterScreenNavigator _navigator;

    public SearchForCharacterScreen(IBookQueries bookQueries, ICharacterQueries characterQueries, ITraitQuieries traitQueries, ISearchForCharacterScreenNavigator navigator)
    {
        _bookQueries = bookQueries;
        _characterQueries = characterQueries;
        _traitQueries = traitQueries;
        _navigator = navigator;
    }

    protected override async Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        var books = await _bookQueries.FetchAllWithAuthorAndStatus(cancellationToken);

        var selectedBook = AnsiConsole.Prompt(
            new SelectionPrompt<Book>()
                .Title("[bold yellow]Select a book[/]")
                .UseConverter(b => b.Title)
                .AddChoices(books)
        );

        var allTraits = await _traitQueries.GetAll(cancellationToken);
        var allCharacters = await _characterQueries.GetByBookIdWithTraits(selectedBook.Id, cancellationToken);
        var selectedTraits = new List<Trait>();
        var searchText = string.Empty;
        var traitCursor = 0;

        await AnsiConsole.Live(BuildDisplay(selectedBook, selectedTraits, searchText, traitCursor, GetMatchingTraits(allTraits, selectedTraits, searchText), allCharacters))
            .StartAsync(async ctx =>
            {
                while (true)
                {
                    var matchingTraits = GetMatchingTraits(allTraits, selectedTraits, searchText);
                    var filteredCharacters = FilterCharacters(allCharacters, selectedTraits);
                    ctx.UpdateTarget(BuildDisplay(selectedBook, selectedTraits, searchText, traitCursor, matchingTraits, filteredCharacters));

                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Escape)
                        break;

                    if (key.Key == ConsoleKey.UpArrow)
                        traitCursor = Math.Max(0, traitCursor - 1);
                    else if (key.Key == ConsoleKey.DownArrow)
                        traitCursor = Math.Min(Math.Max(0, matchingTraits.Count - 1), traitCursor + 1);
                    else if (key.Key == ConsoleKey.Enter && matchingTraits.Count > 0)
                    {
                        selectedTraits.Add(matchingTraits[traitCursor]);
                        searchText = string.Empty;
                        traitCursor = 0;
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (searchText.Length > 0)
                            searchText = searchText[..^1];
                        else if (selectedTraits.Count > 0)
                            selectedTraits.RemoveAt(selectedTraits.Count - 1);
                        traitCursor = 0;
                    }
                    else if (!char.IsControl(key.KeyChar))
                    {
                        searchText += key.KeyChar;
                        traitCursor = 0;
                    }
                }

                await Task.CompletedTask;
            });

        await _navigator.Navigate(cancellationToken);
    }

    private IRenderable BuildDisplay(Book book, List<Trait> selectedTraits, string searchText, int traitCursor, List<Trait> matchingTraits, List<Character> characters)
    {
        var contentWidth = Math.Min(Console.WindowWidth - 4, 120);
        var leftWidth = contentWidth / 3;

        // Header
        var header = new Rule($"[bold yellow] {Markup.Escape(book.Title)} [/]").RuleStyle("yellow");

        // Filtering by — prominent panel across the top
        var filterContent = selectedTraits.Count > 0
            ? string.Join("  ", selectedTraits.Select(t => $"[bold green]• {Markup.Escape(t.Description)}[/]"))
            : "[grey]No traits selected — type to search[/]";

        var filterPanel = new Panel(new Markup(filterContent))
            .Header("[bold white]Filtering By[/]")
            .Padding(2, 0)
            .BorderColor(Color.Yellow)
            .Expand();

        // Left: trait search
        var leftRows = new List<IRenderable>
        {
            new Markup($"  Search: [bold]{Markup.Escape(searchText)}[/]_"),
            new Markup(string.Empty)
        };

        if (matchingTraits.Count > 0)
        {
            foreach (var (trait, i) in matchingTraits.Select((t, i) => (t, i)))
            {
                leftRows.Add(i == traitCursor
                    ? new Markup($"  [bold green]> {Markup.Escape(trait.Description)}[/]")
                    : new Markup($"    {Markup.Escape(trait.Description)}"));
            }
        }
        else if (!string.IsNullOrEmpty(searchText))
        {
            leftRows.Add(new Markup("[grey]  No matching traits[/]"));
        }

        var traitPanel = new Panel(new Rows(leftRows))
            .Header("Traits (Enter to select)")
            .Padding(1, 0)
            .BorderColor(Color.Grey)
            .Expand();

        // Right: characters
        var characterRows = characters.Count > 0
            ? (IRenderable)new Rows(characters.Select(c => (IRenderable)new Markup($"  {Markup.Escape(c.Name)}")).ToArray())
            : new Markup($"[grey]  {(selectedTraits.Count > 0 ? "No characters match" : "All characters")}[/]");

        var characterPanel = new Panel(characterRows)
            .Header($"[bold]Characters[/] [grey]({characters.Count})[/]")
            .Padding(1, 0)
            .BorderColor(Color.Grey)
            .Expand();

        var bodyLayout = new Table()
            .NoBorder()
            .HideHeaders()
            .Width(contentWidth)
            .AddColumn(new TableColumn("").Width(leftWidth).NoWrap())
            .AddColumn(new TableColumn(""));

        bodyLayout.AddRow(traitPanel, characterPanel);

        return new Rows(
            header,
            new Markup(string.Empty),
            filterPanel,
            new Markup(string.Empty),
            bodyLayout,
            new Markup(string.Empty),
            new Rule().RuleStyle("grey"),
            new Markup("[grey]↑↓ navigate   Enter select   Backspace remove   Esc back[/]")
        );
    }

    private static List<Character> FilterCharacters(List<Character> all, List<Trait> selectedTraits)
    {
        if (selectedTraits.Count == 0) return all;

        var selectedIds = selectedTraits.Select(t => t.Id).ToHashSet();
        return all
            .Where(c => selectedIds.All(id => c.Traits!.Any(t => t.Id == id)))
            .ToList();
    }

    private static List<Trait> GetMatchingTraits(List<Trait> all, List<Trait> selected, string search)
    {
        var selectedIds = selected.Select(t => t.Id).ToHashSet();
        return all
            .Where(t => !selectedIds.Contains(t.Id))
            .Where(t => string.IsNullOrEmpty(search) || t.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
            .Take(6)
            .ToList();
    }
}
