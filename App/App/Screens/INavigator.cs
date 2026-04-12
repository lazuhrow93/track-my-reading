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
    ViewCharacterDetails,
    SearchForCharacter,
}

public interface INavigator<TPayload>
{
    Task Navigate(TPayload? payload, CancellationToken cancellationToken);
}