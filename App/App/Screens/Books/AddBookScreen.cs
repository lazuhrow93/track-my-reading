using App.Screens.Catalog;
using Data.Services;
using Spectre.Console;

namespace App.Screens.Books;

public interface IAddBookScreen : IScreen<AddBookScreenInput>
{

}

public record AddBookScreenInput : IScreenInput
{
    public static AddBookScreenInput? Default => null;
}

public class AddBookScreen : IAddBookScreen
{
    private readonly IAddService _addService;
    private readonly IAddBookScreenNavigator _navigator;

    public AddBookScreen(IAddService addService, IAddBookScreenNavigator navigator)
    {
        _addService = addService;
        _navigator = navigator;
    }

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        var name = AnsiConsole.Ask<string>("Who is the author of your book?");
        var book = AnsiConsole.Ask<string>("Title?");

        var result = await _addService.AddBook(book, name, cancellationToken);

        await _navigator.Navigate(Page.ViewCatalog, CatalogScreenInput.Default, cancellationToken);
    }
}
