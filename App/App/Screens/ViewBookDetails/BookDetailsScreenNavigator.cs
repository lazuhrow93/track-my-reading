using App.Screens.Catalog;
using App.Screens.AddCharacter;
using App.Screens.Home;
using App.Screens.ViewCharacterDetailsScreen;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.ViewBookDetails;

public interface IBookDetailsScreenNavigator : INavigator<BookDetailsOnScreenAction>
{
}

public class BookDetailsScreenNavigator : IBookDetailsScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public BookDetailsScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(BookDetailsOnScreenAction? payLoad, CancellationToken cancellationToken)
    {
        if (payLoad == null)
        {
            return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
        }

        if (payLoad.TargetPage == Page.ViewCatalog)
        {
            var screen = _serviceProvider.GetRequiredService<ICatalogScreen>();
            return screen.Show(CatalogScreenInput.Default, cancellationToken);
        }

        if (payLoad.TargetPage == Page.AddCharacter)
        {
            if (payLoad.BookIdToAddCharacter == null)
            {
                throw new ArgumentNullException(nameof(payLoad.BookIdToAddCharacter));
            }

            var screen = _serviceProvider.GetRequiredService<IAddCharacterScreen>();
            var input = new AddCharacterInput() { BookId = payLoad.BookIdToAddCharacter!.Value };
            return screen.Show(input, cancellationToken);
        }

        if (payLoad.TargetPage == Page.ViewCharacterDetails)
        {
            if (payLoad.ChosenCharacter == null)
                throw new ArgumentNullException(nameof(payLoad.ChosenCharacter));

            var screen = _serviceProvider.GetRequiredService<IViewCharacterDetailsScreen>();
            return screen.Show(new ViewCharacterDetailsScreenInput
            {
                CharacterId = payLoad.ChosenCharacter.Id,
                CharacterName = payLoad.ChosenCharacter.Name,
                ShouldClear = true
            }, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<IHomeScreen>().Show(HomeScreenInput.Default, cancellationToken);
    }
}