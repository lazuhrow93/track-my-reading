using Data.Services;
using Spectre.Console;

namespace App.Screens.Books;

public interface IAddBookScreen : IScreen
{

}

public class AddBookScreen : IAddBookScreen
{
    private readonly IAddService _addService;
    private readonly IAddBookScreenNavigator _navigator;

    public AddBookScreen(IAddService addService)
    {
        _addService = addService;
    }

    public async Task Show()
    {
        var name = AnsiConsole.Ask<string>("Who is the author of your book?");
        var book = AnsiConsole.Ask<string>("Title?");

        var result = await _addService.AddBook(book, name, CancellationToken.None);

        await _navigator.Navigate(Page.ViewCatalog);
    }
}
