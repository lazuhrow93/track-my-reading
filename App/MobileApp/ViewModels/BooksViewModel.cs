using System.Collections.ObjectModel;
using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public class BooksViewModel(BooksService booksService)
{
    public ObservableCollection<BookSummary> Books { get; } = [];

    public bool IsLoading { get; private set; }

    public async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        IsLoading = true;
        var books = await booksService.GetBooksAsync(cancellationToken);
        Books.Clear();
        foreach (var book in books)
            Books.Add(book);
        IsLoading = false;
    }
}
