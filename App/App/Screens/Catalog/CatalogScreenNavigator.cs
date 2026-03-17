
using App.Screens.Author;
using App.Screens.Books;
using App.Screens.Characters;
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

    public Task Navigate(Page target, IScreenInput? inputType, CancellationToken cancellationToken)
    {
        if (target == Page.AddBook)
        {
            var screen = _serviceProvider.GetRequiredService<IAddBookScreen>();
            return screen.Show(inputType, cancellationToken);
        }
        else if (target == Page.AddAuthor)
        {
            var screen = _serviceProvider.GetRequiredService<IAddAuthorScreen>();
            return screen.Show(inputType, cancellationToken);
        }
        else if (target == Page.AddCharacter)
        {
            var screen = _serviceProvider.GetRequiredService<IAddCharacterScreen>();
            return screen.Show(inputType, cancellationToken);
        }
        else if (target == Page.BookDetails)
        {
            var screen = _serviceProvider.GetRequiredService<IBookDetailsScreen>();
            return screen.Show(inputType, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<ICatalogScreen>().Show(CatalogScreenInput.Default, cancellationToken);
    }
}
