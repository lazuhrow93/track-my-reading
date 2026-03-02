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

    public Task Navigate(Page target)
    {
        IScreen? resultScreen = target switch
        {
            Page.ViewCatalog => _serviceProvider.GetService<ICatalogScreen>(),
            _ => throw new NotImplementedException("IDK WTF IS HAPPENING")
        };
        return resultScreen!.Show();
    }
}
