namespace App.Screens.Characters;

public interface IViewCharacterDetailsScreen : IScreen<ViewCharacterDetailsScreenInput>
{

}

public class ViewCharacterDetailsScreenInput : ScreenInput, IScreenInput
{
    public string CharacterName { get; set; } = null!;
    public int CharacterId { get; set; }
}

public class ViewCharacterDetailsScreen : Screen<ViewCharacterDetailsScreenInput>, IViewCharacterDetailsScreen
{
    protected override Task OnShow(IScreenInput? input, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
