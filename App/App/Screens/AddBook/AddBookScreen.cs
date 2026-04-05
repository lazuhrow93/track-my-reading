using Data.CRUD.Read;
using Data.Services;
using Spectre.Console;

namespace App.Screens.Books;

public interface IAddBookScreen : IScreen<AddBookScreenInput>
{

}

public class AddBookScreenInput : ScreenInput, IScreenInput
{
    public static AddBookScreenInput? Default => new()
    {
        ShouldClear = false
    };
}

public class AddBookScreenAction
{
    public Page TargetPage { get; set; }
}

public class AddBookScreen : Screen<AddBookScreenInput>, IAddBookScreen
{
    private readonly IAddService _addService;
    private readonly IAuthorQueries _authorQueries;
    private readonly IAddBookScreenNavigator _navigator;

    public AddBookScreen(IAddService addService, IAuthorQueries authorQueries, IAddBookScreenNavigator navigator)
    {
        _addService = addService;
        _authorQueries = authorQueries;
        _navigator = navigator;
    }

    protected override async Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        AnsiConsole.Write(new Rule("[bold green]Add a Book[/]").RuleStyle("grey").LeftJustified());
        AnsiConsole.WriteLine();

        var authors = await _authorQueries.FetchAll(cancellationToken);

        var selectedAuthor = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[grey]Who is the author?[/]")
                .HighlightStyle(new Style(Color.Green, decoration: Decoration.Bold))
                .PageSize(20)
                .AddChoices([.. authors.Select(a => a.Name), "[grey]+ Add Author[/]"]));

        if (selectedAuthor == "[grey]+ Add Author[/]")
        {
            AnsiConsole.Write(new Rule("[dim]New Author[/]").RuleStyle("grey").LeftJustified());
            var newAuthorName = AnsiConsole.Ask<string>("[grey]  Name:[/]");
            await _addService.AddAuthor(newAuthorName, cancellationToken);
            selectedAuthor = newAuthorName;
            AnsiConsole.MarkupLine($"[green]  ✓ Author added[/]");
        }

        AnsiConsole.WriteLine();
        var title = AnsiConsole.Ask<string>("[grey]Title:[/]");

        await _addService.AddBook(title, selectedAuthor, cancellationToken);

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green]✓[/] [bold]{title}[/] by [bold]{selectedAuthor}[/] added to your catalog.");
        AnsiConsole.WriteLine();
        await Task.Delay(1200, cancellationToken);

        var payload = new AddBookScreenAction()
        {
            TargetPage = Page.ViewCatalog
        };

        await _navigator.Navigate(payload, cancellationToken);
    }
}
