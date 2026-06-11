using MobileApp.Screens;

namespace MobileApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("CharactersPage", typeof(CharactersPage));
        Routing.RegisterRoute("AddCharacterPage", typeof(AddCharacterPage));
    }
}
