using App.Screens.Catalog;
using App.Screens.Home;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Author;

public interface IAddAuthorScreenNavigator : INavigator
{

}
public class AddAuthorScreenNavigator : IAddAuthorScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public AddAuthorScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(Page target, IScreenInput? input, CancellationToken cancellationToken)
    {
        if (target == Page.ViewCatalog)
        {
            var catalogScreen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return catalogScreen.Show(input, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}
