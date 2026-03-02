
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Catalog;


public interface ICatalogScreenNavigator : INavigator
{
    
}

public class CatalogScreenNavigator : ICatalogScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public CatalogScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(Page target)
    {
        var targetScreen = target switch
        {
            Page.AddBook => _serviceProvider.GetRequiredService<IAddBookScreen>(),
            _ => throw new NotSupportedException($"Navigation to {target} is not supported.")
        };
    }
}
