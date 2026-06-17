using Microsoft.Maui.Controls.Shapes;
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

    // Stubbed until backend is wired up
    private static readonly List<string> _stubTraits = ["Brave", "Cunning", "Loyal"];
    private static readonly List<string> _stubNotes =
    [
        "First appears in chapter 3.",
        "Has a complicated relationship with the protagonist."
    ];

    public CharacterProfilePage(BooksService booksService)
    {
        InitializeComponent();
        _booksService = booksService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = CharacterName;
        NameLabel.Text = CharacterName;
        DescriptionLabel.Text = CharacterDescription;
        LoadTraits();
        LoadNotes();
    }

    private void LoadTraits()
    {
        TraitsLayout.Children.Clear();
        foreach (var trait in _stubTraits)
        {
            TraitsLayout.Children.Add(new Border
            {
                Padding = new Thickness(12, 6),
                Margin = new Thickness(0, 0, 8, 8),
                StrokeShape = new RoundRectangle { CornerRadius = 16 },
                Content = new Label { Text = trait, FontSize = 13 }
            });
        }
    }

    private void LoadNotes()
    {
        NotesLayout.Children.Clear();
        foreach (var note in _stubNotes)
        {
            NotesLayout.Children.Add(new Border
            {
                Padding = new Thickness(12),
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Content = new Label { Text = note, FontSize = 14 }
            });
        }
    }

    private async void OnEditClicked(object? sender, EventArgs e)
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
            NameLabel.Text = newName;
        }
        else
        {
            await DisplayAlert("Error", "Failed to update character.", "OK");
        }
    }
}
