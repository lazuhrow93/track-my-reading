using Spectre.Console;

namespace App.Screens.Home;

public interface IHomeScreen : IScreen
{

}

public class HomeScreen : IHomeScreen
{
    public static readonly Dictionary<string, Page> _options = new Dictionary<string, Page> 
    {
        { "View Catalog", Page.ViewCatalog }
    };

    private readonly IHomeScreenNavigator _navigator;

    public HomeScreen(IHomeScreenNavigator navigator)
    {
        _navigator = navigator;
    }

    public async Task Show()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Welcome Laz, what can I do for you today?")
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(_options.Keys));

        await _navigator.Navigate(_options[choice]);
    }
}
