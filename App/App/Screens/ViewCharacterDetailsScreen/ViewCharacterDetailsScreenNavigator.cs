using App.Screens.ViewBookDetails;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.ViewCharacterDetailsScreen;

public record ViewCharacterDetailsOnScreenAction(int BookId);

public interface IViewCharacterDetailsScreenNavigator : INavigator<ViewCharacterDetailsOnScreenAction>
{
}

public class ViewCharacterDetailsScreenNavigator : IViewCharacterDetailsScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public ViewCharacterDetailsScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(ViewCharacterDetailsOnScreenAction? payload, CancellationToken cancellationToken)
    {
        var screen = _serviceProvider.GetRequiredService<IBookDetailsScreen>();
        return screen.Show(new BookDetailsInput { BookId = payload?.BookId ?? 0 }, cancellationToken);
    }
}
