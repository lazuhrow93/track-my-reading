using App.Screens.Home;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.SearchForCharacter;

public interface ISearchForCharacterScreenNavigator
{
    Task Navigate(CancellationToken cancellationToken);
}

public class SearchForCharacterScreenNavigator : ISearchForCharacterScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public SearchForCharacterScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(CancellationToken cancellationToken)
    {
        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}
