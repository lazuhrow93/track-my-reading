using System.Collections.ObjectModel;
using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.ViewModels;

public class CharactersViewModel(BooksService booksService)
{
    public ObservableCollection<CharacterSummary> Characters { get; } = [];

    public async Task LoadAsync(int bookId, CancellationToken cancellationToken = default)
    {
        var characters = await booksService.GetCharactersAsync(bookId, cancellationToken);
        Characters.Clear();
        foreach (var character in characters)
            Characters.Add(character);
    }
}
