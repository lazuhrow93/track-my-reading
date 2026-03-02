namespace App.Screens;

public enum Page
{
    Undefined,
    Main,
    ViewCatalog,
    AddAuthor,
    AddBook,
}

public interface INavigator
{
    Task Navigate(Page target);
}
