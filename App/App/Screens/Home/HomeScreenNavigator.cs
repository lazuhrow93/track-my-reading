using App.Screens.Catalog;
using App.Screens.SearchForCharacter;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Home;

public interface IHomeScreenNavigator : INavigator<HomeScreenAction>
{

}

public class HomeScreenNavigator : IHomeScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public HomeScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(HomeScreenAction? payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);

        if (payload.TargetPage == Page.ViewCatalog)
        {
            var screen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return screen.Show(CatalogScreenInput.Default, cancellationToken);
        }

        if (payload.TargetPage == Page.SearchForCharacter)
        {
            var screen = _serviceProvider.GetRequiredService<ISearchForCharacterScreen>();
            return screen.Show(SearchForCharacterScreenInput.Default, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}
