using Data.CRUD.Read;
using Data.Services;
using Database.Entites;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace App.Screens.ViewCharacterDetailsScreen;

public interface IViewCharacterDetailsScreen : IScreen<ViewCharacterDetailsScreenInput>
{
}

public class ViewCharacterDetailsScreenInput : ScreenInput, IScreenInput
{
    public static ViewCharacterDetailsScreenInput Default => new ViewCharacterDetailsScreenInput()
    {
        ShouldClear = false
    };

    public string CharacterName { get; set; } = null!;
    public int CharacterId { get; set; }
}

public class ViewCharacterDetailsScreen : Screen<ViewCharacterDetailsScreenInput>, IViewCharacterDetailsScreen
{
    private readonly ICharacterQueries _characterQueries;
    private readonly IAddService _addService;
    private readonly IViewCharacterDetailsScreenNavigator _navigator;

    public ViewCharacterDetailsScreen(ICharacterQueries characterQueries, IAddService addService, IViewCharacterDetailsScreenNavigator navigator)
    {
        _characterQueries = characterQueries;
        _addService = addService;
        _navigator = navigator;
    }

    protected override async Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        if (input is not ViewCharacterDetailsScreenInput parsedInput)
            throw new ArgumentNullException();

        var character = await _characterQueries.GetByIdWithTraitsAndBook(parsedInput.CharacterId, cancellationToken);

        ArgumentNullException.ThrowIfNull(character);

        bool exit = false;
        while (!exit)
        {
            AnsiConsole.Clear();
            RenderProfile(character);

            var currentWidth = Console.WindowWidth;
            var currentHeight = Console.WindowHeight;

            while (!exit)
            {
                if (Console.WindowWidth != currentWidth || Console.WindowHeight != currentHeight)
                    break;

                if (!Console.KeyAvailable)
                {
                    Thread.Sleep(50);
                    continue;
                }

                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Escape)
                {
                    exit = true;
                    break;
                }

                if (key.Key == ConsoleKey.N)
                {
                    AnsiConsole.WriteLine();
                    var noteText = AnsiConsole.Ask<string>("[bold]Note:[/]");

                    if (!string.IsNullOrWhiteSpace(noteText))
                    {
                        //TODO: Dont refetch, just add note on the screen if successful add. 
                        await _addService.AddNote(noteText, character.Id, cancellationToken);
                        character = await _characterQueries.GetByIdWithTraitsAndBook(parsedInput.CharacterId, cancellationToken);

                        ArgumentNullException.ThrowIfNull(character);
                    }

                    break;
                }
            }
        }

        await _navigator.Navigate(new ViewCharacterDetailsOnScreenAction(character.BookId), cancellationToken);
    }

    private void RenderProfile(Character character)
    {
        var contentWidth = Math.Min(Console.WindowWidth - 4, 120);

        AnsiConsole.Write(new Rule("[bold yellow] Profile [/]").RuleStyle("yellow"));
        AnsiConsole.WriteLine();

        var infoGrid = new Grid()
            .AddColumn(new GridColumn().Width(16).NoWrap())
            .AddColumn(new GridColumn());

        infoGrid.AddRow("[grey]Name[/]", $"[bold white]{Markup.Escape(character.Name)}[/]");
        infoGrid.AddRow("[grey]Book[/]", $"[bold]{Markup.Escape(character.Book?.Title ?? "Unknown")}[/]");
        infoGrid.AddRow("[grey]Added[/]", $"[grey]{character.CreatedUtc:MMMM dd, yyyy}[/]");

        var infoPanel = new Panel(infoGrid)
            .Padding(2, 1)
            .BorderColor(Color.Grey);

        if (!string.IsNullOrWhiteSpace(character.Description))
        {
            var aboutPanel = new Panel(new Markup(Markup.Escape(character.Description)))
                .Header("[bold]About[/]")
                .Padding(2, 1)
                .BorderColor(Color.Grey);

            var sideLayout = new Table()
                .NoBorder()
                .HideHeaders()
                .Width(contentWidth)
                .AddColumn(new TableColumn("").NoWrap())
                .AddColumn(new TableColumn(""));

            sideLayout.AddRow(infoPanel, aboutPanel);
            AnsiConsole.Write(new Align(sideLayout, HorizontalAlignment.Center));
        }
        else
        {
            AnsiConsole.Write(new Align(infoPanel, HorizontalAlignment.Center));
        }

        if (character.Traits?.Count > 0)
        {
            AnsiConsole.WriteLine();

            var traitsRows = character.Traits
                .Select(t => (IRenderable)new Markup($"  [green]•[/] {Markup.Escape(t.Description)}"))
                .ToArray();

            var traitsPanel = new Panel(new Rows(traitsRows))
                .Header("[bold]Traits[/]")
                .Padding(2, 0)
                .BorderColor(Color.Grey);

            AnsiConsole.Write(new Align(WrapInTable(traitsPanel, contentWidth), HorizontalAlignment.Center));
        }

        if (character.Notes?.Count > 0)
        {
            AnsiConsole.WriteLine();

            var noteRows = character.Notes
                .OrderBy(n => n.CreatedUtc)
                .Select(n => (IRenderable)new Markup($"  [grey]{n.CreatedUtc:MMM dd, yyyy}[/]  {Markup.Escape(n.Value)}"))
                .ToArray();

            var notesPanel = new Panel(new Rows(noteRows))
                .Header("[bold]Notes[/]")
                .Padding(2, 0)
                .BorderColor(Color.Grey);

            AnsiConsole.Write(new Align(WrapInTable(notesPanel, contentWidth), HorizontalAlignment.Center));
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule().RuleStyle("grey"));
        AnsiConsole.Write(new Align(new Markup("[grey]N  add note   Esc  back[/]"), HorizontalAlignment.Center));
        AnsiConsole.WriteLine();
    }

    private static Table WrapInTable(IRenderable content, int width)
    {
        return new Table()
            .NoBorder()
            .HideHeaders()
            .Width(width)
            .AddColumn(new TableColumn(""))
            .AddRow(content);
    }
}
