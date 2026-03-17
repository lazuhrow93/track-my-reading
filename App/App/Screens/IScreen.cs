namespace App.Screens;

public interface IScreen<TInputType>
    where TInputType : IScreenInput
{
    Task Show(IScreenInput? input, CancellationToken cancellationToken);
}

public interface IScreenInput
{
}