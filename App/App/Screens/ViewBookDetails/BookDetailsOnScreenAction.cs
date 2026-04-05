using Database.Entites;

namespace App.Screens.ViewBookDetails;

public record BookDetailsOnScreenAction
{
    public Character? ChosenCharacter { get; set; }

    public int? BookIdToAddCharacter { get; set; }

    public Page? TargetPage { get; set; }

    public BookDetailsOnScreenAction() { }

    public BookDetailsOnScreenAction(Character? chosenCharacter, Page? redirect)
    {
        ChosenCharacter = chosenCharacter;
        TargetPage = redirect;
    }

    public static BookDetailsOnScreenAction AddCharacterForBook(int bookId) => new BookDetailsOnScreenAction()
    {
        BookIdToAddCharacter = bookId,
        TargetPage = Page.AddCharacter
    };
}
