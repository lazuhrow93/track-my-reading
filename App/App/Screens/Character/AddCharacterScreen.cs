using App.Screens.Books;
using Data.Services;
using Spectre.Console;

namespace App.Screens.Characters;

public interface IAddCharacterScreen : IScreen<AddCharacterScreenInput>
{

}

public class AddCharacterScreenInput : IScreenInput
{
    public static AddCharacterScreenInput? Default => null;

    public required int BookId { get; set; }
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
        if (input is not AddCharacterScreenInput addCharacterInput)
        {
            throw new ArgumentException($"Expected input of type {typeof(AddCharacterScreenInput).FullName}", nameof(input));
        }

        AnsiConsole.Clear();
        var name = AnsiConsole.Ask<string>("What is the character's name?");
        var description = AnsiConsole.Ask<string>("Description? (leave blank to skip)");

        await _addService.AddCharacter(name, string.IsNullOrWhiteSpace(description) ? null : description, addCharacterInput.BookId, cancellationToken);

        await _navigator.Navigate(Page.BookDetails, new BookDetailsScreenInput()
        {
            BookId = addCharacterInput.BookId
        }, cancellationToken);
    }
}
