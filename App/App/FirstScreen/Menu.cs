using Spectre.Console;

namespace App.FirstScreen;

public class Menu
{
    public void Start()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Welcome Laz, what can I do for you today?")
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(MenuOptions.All));
    }
}
