using Spectre.Console;

namespace App.Screens.Books;

public interface IAddBookScreen : IScreen
{

}

public class AddBookScreen : IAddBookScreen
{
    private readonly IAddService _addService;

    public AddBookScreen(IAddService addService)
    {
        _addService = addService;
    }

    public Task Show()
    {
        var name = AnsiConsole.Ask<string>("Who is the author of your book?");
        var book = AnsiConsole.Ask<string>("Title?");
    }
}
