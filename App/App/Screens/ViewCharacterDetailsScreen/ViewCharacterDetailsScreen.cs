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
    private readonly ITraitQuieries _traitQueries;
    private readonly IAddService _addService;
    private readonly IViewCharacterDetailsScreenNavigator _navigator;

    public ViewCharacterDetailsScreen(ICharacterQueries characterQueries, ITraitQuieries traitQueries, IAddService addService, IViewCharacterDetailsScreenNavigator navigator)
    {
        _characterQueries = characterQueries;
        _traitQueries = traitQueries;
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

                if (key.Key == ConsoleKey.T)
                {
                    var allTraits = await _traitQueries.GetAll(cancellationToken);
                    var assignedIds = character.Traits?.Select(t => t.Id).ToHashSet() ?? [];
                    var available = allTraits.Where(t => !assignedIds.Contains(t.Id)).ToList();

                    if (available.Count == 0)
                        break;

                    var selected = AnsiConsole.Prompt(
                        new SelectionPrompt<Trait>()
                            .Title("[bold]Select a trait[/]")
                            .EnableSearch()
                            .UseConverter(t => t.Description)
                            .AddChoices(available)
                    );

                    await _addService.AddTraitToCharacter(character.Id, selected.Id, cancellationToken);
                    character = await _characterQueries.GetByIdWithTraitsAndBook(parsedInput.CharacterId, cancellationToken);
                    ArgumentNullException.ThrowIfNull(character);
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
            .Header("[bold]Id[/]")
            .Padding(2, 1)
            .BorderColor(Color.Grey)
            .Expand();

        var totalTraits = character.Traits?.Count ?? 0;

        var traitsContent = totalTraits > 0
            ? (IRenderable)new Rows(character.Traits.Select(t => (IRenderable)new Markup($"  [green]•[/] {Markup.Escape(t.Description)}")).ToArray())
            : new Markup(string.Empty);

        var traitsPanel = new Panel(traitsContent)
            .Header("[bold]Traits[/] [grey][[T: Add Trait]][/]")
            .Padding(1, 0)
            .BorderColor(Color.Grey)
            .Expand();

        var leftWidth = contentWidth / 3;
        var leftColumn = new Rows(infoPanel, traitsPanel);

        if (!string.IsNullOrWhiteSpace(character.Description))
        {
            var aboutContent = new Rows(
                new Rule("[bold]About[/]").LeftJustified().RuleStyle("grey"),
                new Markup(Markup.Escape(character.Description))
            );

            var sideLayout = new Table()
                .NoBorder()
                .HideHeaders()
                .Width(contentWidth)
                .AddColumn(new TableColumn("").Width(leftWidth).NoWrap())
                .AddColumn(new TableColumn(""));

            sideLayout.AddRow(leftColumn, aboutContent);
            AnsiConsole.Write(new Align(sideLayout, HorizontalAlignment.Center));
        }
        else
        {
            AnsiConsole.Write(new Align(WrapInTable(leftColumn, leftWidth), HorizontalAlignment.Center));
        }

        AnsiConsole.WriteLine();

        var noteRows = character.Notes?
            .OrderBy(n => n.CreatedUtc)
            .Select(n => (IRenderable)new Markup($"  [grey]{n.CreatedUtc:MMM dd, yyyy}[/]  {Markup.Escape(n.Value)}"))
            .ToArray() ?? [];

        var notesPanel = new Panel(noteRows.Length > 0 ? new Rows(noteRows) : new Markup(string.Empty))
            .Header("[bold]Notes[/] [grey][[N]] to add[/]")
            .Padding(2, 0)
            .BorderColor(Color.Grey)
            .Expand();

        AnsiConsole.Write(new Align(WrapInTable(notesPanel, contentWidth), HorizontalAlignment.Center));

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule().RuleStyle("grey"));
        AnsiConsole.MarkupLine("[grey][[Esc: Back]][/]");
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
