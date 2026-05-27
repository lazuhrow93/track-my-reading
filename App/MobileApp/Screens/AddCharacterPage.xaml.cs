using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.Screens;

[QueryProperty(nameof(BookId), "bookId")]
public partial class AddCharacterPage : ContentPage
{
    private readonly BooksService _booksService;

    public int BookId { get; set; }

    public AddCharacterPage(BooksService booksService)
    {
        InitializeComponent();
        _booksService = booksService;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var model = new AddCharacterModel
        {
            Name = NameEntry.Text,
            Description = DescriptionEntry.Text,
            BookId = BookId
        };

        var success = await _booksService.AddCharacterAsync(model);
        if (success)
            await Shell.Current.GoToAsync("..");
        else
            await DisplayAlert("Error", "Failed to add character.", "OK");
    }
}
