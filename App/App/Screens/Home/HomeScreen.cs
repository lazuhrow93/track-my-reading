using App.Screens.Catalog;
using Spectre.Console;

namespace App.Screens.Home;

public interface IHomeScreen : IScreen<HomeScreenInput>
{

}

public record HomeScreenInput : IScreenInput
{
    public static HomeScreenInput? Default => null;
}

public class HomeScreen : IHomeScreen
{
    private static readonly Dictionary<string, Page> _options = new Dictionary<string, Page> 
    {
        { "View Catalog", Page.ViewCatalog }
    };

    private readonly IHomeScreenNavigator _navigator;

    public HomeScreen(IHomeScreenNavigator navigator)
    {
        _navigator = navigator;
    }

    public Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        AnsiConsole.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Welcome Laz, what can I do for you today?")
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(_options.Keys));

        var target = _options[choice];

        if (target == Page.ViewCatalog)
        {
            return _navigator.Navigate(target, CatalogScreenInput.Default, cancellationToken);
        }
        else
        {
            throw new NotImplementedException();
        }
        
    }
}
