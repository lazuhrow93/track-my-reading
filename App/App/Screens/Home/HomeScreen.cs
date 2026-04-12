using App.Screens.Catalog;
using Spectre.Console;

namespace App.Screens.Home;

public interface IHomeScreen : IScreen<HomeScreenInput>
{

}

public class HomeScreenInput : ScreenInput, IScreenInput
{
    public static HomeScreenInput? Default => null;
}

public class HomeScreenAction
{
    public Page TargetPage { get; set; }
}

public class HomeScreen : Screen<HomeScreenInput>, IHomeScreen
{
    private static readonly Dictionary<string, Page> _options = new Dictionary<string, Page>
    {
        { "View Your Catalog", Page.ViewCatalog },
        { "Search For Character", Page.SearchForCharacter }
    };

    private readonly IHomeScreenNavigator _navigator;

    public HomeScreen(IHomeScreenNavigator navigator)
    {
        _navigator = navigator;
    }

    protected override Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Welcome Laz, what can I do for you today?")
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(_options.Keys));


        var target = _options[choice];

        return _navigator.Navigate(new HomeScreenAction() { TargetPage = target }, cancellationToken);
    }
}
