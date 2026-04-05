namespace App.Screens.ViewBookDetails;

public record BookDetailsOnScreenAction
{
    public int? ChosenCharacterId { get; set; }

    public int? BookIdToAddCharacter { get; set; }

    public Page? TargetPage { get; set; }

    public BookDetailsOnScreenAction() { }

    public BookDetailsOnScreenAction(int? chosenCharacterId, Page? redirect)
    {
        ChosenCharacterId = chosenCharacterId;
        TargetPage = redirect;
    }

    public static BookDetailsOnScreenAction AddCharacterForBook(int bookId) => new BookDetailsOnScreenAction()
    {
        BookIdToAddCharacter = bookId,
        TargetPage = Page.AddCharacter
    };
}
