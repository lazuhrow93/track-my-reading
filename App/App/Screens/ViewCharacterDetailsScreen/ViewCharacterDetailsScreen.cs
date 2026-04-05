namespace App.Screens.ViewCharacterDetailsScreen;

public interface IViewCharacterDetailsScreen : IScreen<ViewCharacterDetailsScreenInput>
{

}

public class ViewCharacterDetailsScreenInput : ScreenInput, IScreenInput
{
    public static ViewCharacterDetailsScreenInput Default => new ViewCharacterDetailsScreenInput()
    {
        ShouldClear = false
    };

    public string CharacterName { get; set; } = null!;
    public int CharacterId { get; set; }
}

public class ViewCharacterDetailsScreen : Screen<ViewCharacterDetailsScreenInput>, IViewCharacterDetailsScreen
{
    protected override Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        //create a table with descriptions of the Character
        //I want to see things like hair color, eye color, height, weight, etc.
        throw new NotImplementedException();
    }
}
