using App.Screens.Catalog;
using App.Screens.Home;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Books;

public interface IBookDetailsScreenNavigator : INavigator
{
}

public class BookDetailsScreenNavigator : IBookDetailsScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;
    public BookDetailsScreenNavigator(IServiceProvider serviceProvider)
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

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}