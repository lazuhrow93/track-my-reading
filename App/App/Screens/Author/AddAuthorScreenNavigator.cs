using App.Screens.Catalog;
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

    public Task Navigate(Page target)
    {
        IScreen screen = target switch
        {
            Page.ViewCatalog => _serviceProvider.GetRequiredService<ICatalogScreen>(),
            _ => throw new InvalidOperationException($"Navigation to {target} is not supported from AddAuthorScreen.")
        };

        return screen.Show();
    }
}
