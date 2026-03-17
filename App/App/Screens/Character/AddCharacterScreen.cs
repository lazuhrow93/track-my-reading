using Data.Services;
using Spectre.Console;

namespace App.Screens.Characters;

public interface IAddCharacterScreen : IScreen<AddCharacterScreenInput>
{

}

public class AddCharacterScreenInput : IScreenInput
{
    public static AddCharacterScreenInput? Default => null;
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

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        var bookId = AnsiConsole.Ask<int>("What is the Book ID for this character?");
        var name = AnsiConsole.Ask<string>("What is the character's name?");
        var description = AnsiConsole.Ask<string>("Description? (leave blank to skip)");

        await _addService.AddCharacter(name, string.IsNullOrWhiteSpace(description) ? null : description, bookId, cancellationToken);

        await _navigator.Navigate(Page.ViewCatalog, input, cancellationToken);
    }
}
