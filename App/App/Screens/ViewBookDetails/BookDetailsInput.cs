namespace App.Screens.ViewBookDetails;

public class BookDetailsInput : ScreenInput, IScreenInput
{
    public IScreenInput? Default => null;

    public int BookId { get; set; }
}
