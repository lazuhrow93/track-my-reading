using Spectre.Console;

namespace App.Screens;

public abstract class Screen<TInputType> : IScreen<TInputType>
    where TInputType : IScreenInput
{

    protected virtual void ApplyStyle()
    {
        AnsiConsole.Background = Color.Black;
    }

    public async Task Show(IScreenInput? input, CancellationToken cancellationToken)
    {
        ApplyStyle();

        if (input?.ShouldClear == true)
        {
            AnsiConsole.Clear();
        }

        await OnShow(input, cancellationToken);
    }

    protected abstract Task OnShow(IScreenInput? input, CancellationToken cancellationToken);
}
