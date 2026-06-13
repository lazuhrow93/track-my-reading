using MobileApp.Services;

namespace MobileApp.Screens;

[QueryProperty(nameof(CharacterId), "characterId")]
[QueryProperty(nameof(CharacterName), "characterName")]
[QueryProperty(nameof(CharacterDescription), "characterDescription")]
public partial class CharacterProfilePage : ContentPage
{
    private readonly BooksService _booksService;

    public int CharacterId { get; set; }
    public string CharacterName { get; set; } = string.Empty;
    public string CharacterDescription { get; set; } = string.Empty;

    public CharacterProfilePage(BooksService booksService)
    {
        InitializeComponent();
        _booksService = booksService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = CharacterName;
        DescriptionLabel.Text = CharacterDescription;
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var newName = await DisplayPromptAsync(
            "Edit Character",
            "Enter a new name:",
            initialValue: CharacterName,
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(newName) || newName == CharacterName)
            return;

        var success = await _booksService.EditCharacterAsync(CharacterId, newName);
        if (success)
        {
            CharacterName = newName;
            Title = newName;
        }
        else
        {
            await DisplayAlert("Error", "Failed to update character.", "OK");
        }
    }
}
