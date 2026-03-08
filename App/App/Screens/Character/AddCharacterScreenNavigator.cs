using App.Screens.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Character;

public interface IAddCharacterScreenNavigator : INavigator
{

}

public class AddCharacterScreenNavigator : IAddCharacterScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public AddCharacterScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(Page target)
    {
        IScreen screen = target switch
        {
            Page.ViewCatalog => _serviceProvider.GetRequiredService<ICatalogScreen>(),
            _ => throw new InvalidOperationException($"Navigation to {target} is not supported from AddCharacterScreen.")
        };

        return screen.Show();
    }
}
