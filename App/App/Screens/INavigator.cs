namespace App.Screens;

public enum Page
{
    Undefined,
    Main,
    ViewCatalog,
    AddAuthor,
    AddBook,
    AddCharacter,
    BookDetails,
}

public interface INavigator
{
    Task Navigate(Page target, IScreenInput? input, CancellationToken cancellationToken);
}
