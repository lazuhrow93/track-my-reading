using MobileApp.Services;

namespace MobileApp;

public partial class App : Application
{
    private readonly BooksService _booksService;

    public App(BooksService booksService)
    {
        InitializeComponent();
        _booksService = booksService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());
        window.Created += OnWindowCreated;
        return window;
    }

    private async void OnWindowCreated(object? sender, EventArgs e)
    {
        var reachable = await _booksService.IsHealthyAsync();
        if (!reachable)
            await Shell.Current.DisplayAlertAsync("Connection Error", "Cannot reach the server. Make sure the API is running.", "OK");
    }
}