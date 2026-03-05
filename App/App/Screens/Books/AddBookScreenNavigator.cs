
using App.Screens.Catalog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace App.Screens.Books;

public interface IAddBookScreenNavigator : INavigator
{

}

public class AddBookScreenNavigator : IAddBookScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public AddBookScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(Page target)
    {
        IScreen screen = target switch
        {
            Page.ViewCatalog => _serviceProvider.GetRequiredService<ICatalogScreen>(),
            _ => throw new InvalidOperationException($"Navigation to {target} is not supported.")
        };
        return screen.Show();
    }
}
