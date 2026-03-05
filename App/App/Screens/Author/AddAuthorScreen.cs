
using Data.Services;
using Spectre.Console;

namespace App.Screens.Author;

public interface IAddAuthorScreen : IScreen
{

}

public class AddAuthorScreen : IAddAuthorScreen
{
    private readonly IAddService _addService;
    private readonly IAddAuthorScreenNavigator _navigator;

    public AddAuthorScreen(IAddService addService, IAddAuthorScreenNavigator navigator)
    {
        _addService = addService;
        _navigator = navigator;
    }

    public async Task Show()
    {
        var name = AnsiConsole.Ask<string>("What is the name of the author you want to add?");
        await _addService.AddAuthor(name, CancellationToken.None);

        await _navigator.Navigate(Page.ViewCatalog);
    }
}
