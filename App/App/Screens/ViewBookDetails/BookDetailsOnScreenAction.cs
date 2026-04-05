using Database.Entites;

namespace App.Screens.ViewBookDetails;

public record BookDetailsOnScreenAction
{
    public Character? ChosenCharacter { get; set; }

    public int? BookIdToAddCharacter { get; set; }

    public Page? TargetPage { get; set; }

    public BookDetailsOnScreenAction() { }

    public static BookDetailsOnScreenAction BackToCatalog() => new BookDetailsOnScreenAction() { TargetPage = Page.ViewCatalog };

    public static BookDetailsOnScreenAction ViewCharacterDetails(Character character) => new BookDetailsOnScreenAction()
    {
        ChosenCharacter = character,
        TargetPage = Page.ViewCharacterDetails
    };

    public static BookDetailsOnScreenAction AddCharacterForBook(int bookId) => new BookDetailsOnScreenAction()
    {
        BookIdToAddCharacter = bookId,
        TargetPage = Page.AddCharacter
    };
}
