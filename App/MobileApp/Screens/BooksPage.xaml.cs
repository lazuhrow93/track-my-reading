using MobileApp.ViewModels;

namespace MobileApp.Screens;

public partial class BooksPage : ContentPage
{
    private readonly BooksViewModel _viewModel;

    public BooksPage(BooksViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private async void OnViewCharactersClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: int bookId })
            await Shell.Current.GoToAsync($"CharactersPage?bookId={bookId}");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        try
        {
            await _viewModel.LoadAsync();
            BooksCollection.ItemsSource = _viewModel.Books;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load books: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }
}
