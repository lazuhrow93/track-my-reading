namespace App.Screens.AddCharacter;

public class AddCharacterOnScreenAction
{
    public Page TargetPage { get; set; }

    public int? BookIdForBookDetails { get; set; }

    public static AddCharacterOnScreenAction ViewDetails(int bookId) => new AddCharacterOnScreenAction() { TargetPage = Page.BookDetails, BookIdForBookDetails = bookId };
}
