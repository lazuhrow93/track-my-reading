namespace App.Screens;

public enum Page
{
    Undefined,
    Main,
    ViewCatalog,
    AddAuthor,
    AddBook,
    DeleteBook,
    EditBook,
    AddCharacter,
    BookDetails,
}

public interface INavigator
{
    Task Navigate(Page target, IScreenInput? input, CancellationToken cancellationToken);
}
