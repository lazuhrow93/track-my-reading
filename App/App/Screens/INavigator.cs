namespace App.Screens;

public enum Page
{
    Undefined,
    Main,
    ViewCatalog,
    AddBook,
}

public interface INavigator
{
    Task Navigate(Page target);
}
