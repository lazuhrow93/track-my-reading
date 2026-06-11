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

    private async void OnTryAgainClicked(object sender, EventArgs e) => await LoadBooksAsync();

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadBooksAsync();
    }

    private async Task LoadBooksAsync()
    {
        ErrorPanel.IsVisible = false;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        try
        {
            await _viewModel.LoadAsync();
            BooksCollection.ItemsSource = _viewModel.Books;
        }
        catch
        {
            ErrorPanel.IsVisible = true;
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }
}
