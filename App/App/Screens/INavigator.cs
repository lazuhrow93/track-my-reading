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

public interface INavigator<TPayload>
{
    Task Navigate(TPayload? payload, CancellationToken cancellationToken);
}