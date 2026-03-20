using App.Screens.Books;
using App.Screens.Catalog;
using App.Screens.Home;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Characters;

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

    public Task Navigate(Page target, IScreenInput? inputType, CancellationToken cancellationToken)
    {
        if (target == Page.ViewCatalog)
        {
            var screen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return screen.Show(inputType, cancellationToken);
        }

        if (target == Page.BookDetails)
        {
            var screen = _serviceProvider.GetRequiredService<IBookDetailsScreen>();
            return screen.Show(inputType, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}
