namespace App.Screens.AddCharacter;

public class AddCharacterInput : ScreenInput, IScreenInput
{
    public static AddCharacterInput? Default => null;

    public required int BookId { get; set; }
}
