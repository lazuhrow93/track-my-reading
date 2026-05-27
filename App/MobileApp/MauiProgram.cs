using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MobileApp.Screens;
using MobileApp.Services;
using MobileApp.ViewModels;

namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var assembly = typeof(App).Assembly;
        using var stream = assembly.GetManifestResourceStream("MobileApp.appsettings.json");
        if (stream is not null)
            builder.Configuration.AddJsonStream(stream);

        var baseUrl = builder.Configuration["Api:BaseUrl"];

        builder.Services.AddHttpClient<BooksService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });

        builder.Services.AddTransient<BooksViewModel>();
        builder.Services.AddTransient<BooksPage>();
        builder.Services.AddTransient<CharactersViewModel>();
        builder.Services.AddTransient<CharactersPage>();
        builder.Services.AddTransient<AddCharacterPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
