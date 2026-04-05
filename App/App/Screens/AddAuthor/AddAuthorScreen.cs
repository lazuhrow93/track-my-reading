using Data.Services;
using Spectre.Console;

namespace App.Screens.Author;

public interface IAddAuthorScreen : IScreen<AddAuthorScreenInput>
{

}

public class AddAuthorScreen : Screen<AddAuthorScreenInput>, IAddAuthorScreen
{
    private readonly IAddService _addService;
    private readonly IAddAuthorNavigator _navigator;

    public AddAuthorScreen(IAddService addService, IAddAuthorNavigator navigator)
    {
        _addService = addService;
        _navigator = navigator;
    }

    protected override async Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        var name = AnsiConsole.Ask<string>("What is the name of the author you want to add?");
        await _addService.AddAuthor(name, cancellationToken);


        await _navigator.Navigate(new() { TargetPage = Page.ViewCatalog }, cancellationToken);
    }
}
