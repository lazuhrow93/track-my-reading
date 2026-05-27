using System.Net.Http.Json;
using MobileApp.Models;

namespace MobileApp.Services;

public class BooksService(HttpClient httpClient)
{
    public async Task<List<BookSummary>> GetBooksAsync(CancellationToken cancellationToken = default)
    {
        var books = await httpClient.GetFromJsonAsync<List<BookSummary>>("/books", cancellationToken);
        return books ?? [];
    }

    public async Task<List<CharacterSummary>> GetCharactersAsync(int bookId, CancellationToken cancellationToken = default)
    {
        var characters = await httpClient.GetFromJsonAsync<List<CharacterSummary>>($"/book/{bookId}/characters", cancellationToken);
        return characters ?? [];
    }

    public async Task<bool> AddCharacterAsync(AddCharacterModel model, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/book/character", model, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
