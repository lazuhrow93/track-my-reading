using MobileApp.ViewModels;

namespace MobileApp.Screens;

[QueryProperty(nameof(BookId), "bookId")]
public partial class CharactersPage : ContentPage
{
    private readonly CharactersViewModel _viewModel;

    public int BookId { get; set; }

    public CharactersPage(CharactersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        try
        {
            await _viewModel.LoadAsync(BookId);
            CharactersCollection.ItemsSource = _viewModel.Characters;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load characters: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }
}
