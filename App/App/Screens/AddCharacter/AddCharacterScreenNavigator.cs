using App.Screens.Catalog;
using App.Screens.Home;
using App.Screens.ViewBookDetails;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.AddCharacter;

public interface IAddCharacterScreenNavigator : INavigator<AddCharacterOnScreenAction>
{

}

public class AddCharacterScreenNavigator : IAddCharacterScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public AddCharacterScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(AddCharacterOnScreenAction? payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);

        if (payload.TargetPage == Page.ViewCatalog)
        {
            var screen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return screen.Show(CatalogScreenInput.Default, cancellationToken);
        }

        if (payload.TargetPage == Page.BookDetails)
        {
            if (payload.BookIdForBookDetails == null)
            {
                throw new ArgumentNullException(nameof(payload), "BookIdForBookDetails");
            }

            var input = new BookDetailsInput
            {
                BookId = payload.BookIdForBookDetails.Value
            };

            var screen = _serviceProvider.GetRequiredService<IBookDetailsScreen>();
            return screen.Show(input, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}
