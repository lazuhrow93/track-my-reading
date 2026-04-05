using App.Screens.Books;
using App.Screens.ViewBookDetails;
using Microsoft.Extensions.DependencyInjection;

namespace App.Screens.Catalog;


public interface ICatalogScreenNavigator : INavigator<CatalogScreenAction>
{
    
}

public class CatalogScreenNavigator : ICatalogScreenNavigator
{
    private readonly IServiceProvider _serviceProvider;

    public CatalogScreenNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Navigate(CatalogScreenAction? payload, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payload);

        if (payload.TargetPage == Page.AddBook)
        {
            var screen = _serviceProvider.GetRequiredService<IAddBookScreen>();
            return screen.Show(AddBookScreenInput.Default, cancellationToken);
        }
        //else if (target == Page.AddAuthor)
        //{
        //    var screen = _serviceProvider.GetRequiredService<IAddAuthorScreen>();
        //    return screen.Show(inputType, cancellationToken);
        //}
        //else if (target == Page.AddCharacter)
        //{
        //    var screen = _serviceProvider.GetRequiredService<IAddCharacterScreen>();
        //    return screen.Show(inputType, cancellationToken);
        //}
        else if (payload.TargetPage == Page.BookDetails)
        {
            if (!payload.BookIdForBookDetails.HasValue)
            {
                throw new ArgumentException("BookIdForBookDetails must have a value when TargetPage is BookDetails.");
            }

            var inputType = new BookDetailsInput()
            {
                BookId = payload.BookIdForBookDetails.Value
            };

            var screen = _serviceProvider.GetRequiredService<IBookDetailsScreen>();
            return screen.Show(inputType, cancellationToken);
        }

        return _serviceProvider.GetRequiredService<ICatalogScreen>().Show(CatalogScreenInput.Default, cancellationToken);
    }
}
