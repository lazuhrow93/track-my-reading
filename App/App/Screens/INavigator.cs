namespace App.Screens;

public enum Page
{
    Undefined,
    Main,
    ViewCatalog,
    AddAuthor,
    AddBook,
    AddCharacter,
}

public interface INavigator
{
    Task Navigate(Page target);
}
