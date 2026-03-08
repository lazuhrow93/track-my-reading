
using App.Screens.Author;
using App.Screens.Books;
using App.Screens.Character;
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
        IScreen targetScreen = target switch
        {
            Page.AddBook => _serviceProvider.GetRequiredService<IAddBookScreen>(),
            Page.AddAuthor => _serviceProvider.GetRequiredService<IAddAuthorScreen>(),
            Page.AddCharacter => _serviceProvider.GetRequiredService<IAddCharacterScreen>(),
            _ => throw new NotSupportedException($"Navigation to {target} is not supported.")
        };

        return targetScreen.Show();
    }
}
