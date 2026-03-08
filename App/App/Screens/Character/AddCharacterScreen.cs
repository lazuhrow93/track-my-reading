using Data.Services;
using Spectre.Console;

namespace App.Screens.Character;

public interface IAddCharacterScreen : IScreen
{

}

public class AddCharacterScreen : IAddCharacterScreen
{
    private readonly IAddService _addService;
    private readonly IAddCharacterScreenNavigator _navigator;

    public AddCharacterScreen(IAddService addService, IAddCharacterScreenNavigator navigator)
    {
        _addService = addService;
        _navigator = navigator;
    }

    public async Task Show()
    {
        var bookId = AnsiConsole.Ask<int>("What is the Book ID for this character?");
        var name = AnsiConsole.Ask<string>("What is the character's name?");
        var description = AnsiConsole.Ask<string>("Description? (leave blank to skip)");

        await _addService.AddCharacter(name, string.IsNullOrWhiteSpace(description) ? null : description, bookId, CancellationToken.None);

        await _navigator.Navigate(Page.ViewCatalog);
    }
}
