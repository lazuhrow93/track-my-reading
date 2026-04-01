namespace App.Screens;

public interface IScreenInput
{
    bool ShouldClear { get; init; }
}

public abstract class ScreenInput : IScreenInput
{
    public bool ShouldClear { get; init; } = true;
}