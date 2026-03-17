using App.Screens.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Home;

public interface IHomeScreenNavigator : INavigator
{

}

public class HomeScreenNavigator : IHomeScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public HomeScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(Page target, IScreenInput? input, CancellationToken cancellationToken)
    {
        if (target == Page.ViewCatalog)
        {
            var screen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return screen.Show(input, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(input, cancellationToken);
    }
}
