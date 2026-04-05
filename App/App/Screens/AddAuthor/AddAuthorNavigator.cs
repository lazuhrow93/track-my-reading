using App.Screens.Catalog;
using App.Screens.Home;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Author;

public interface IAddAuthorNavigator : INavigator<AddAuthorOnScreenAction>
{

}
public class AddAuthorNavigator : IAddAuthorNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public AddAuthorNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(AddAuthorOnScreenAction? payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);
        
        if (payload.TargetPage == Page.ViewCatalog)
        {
            var catalogScreen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return catalogScreen.Show(CatalogScreenInput.Default, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}
