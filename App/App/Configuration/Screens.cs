using App.Screens.Author;
using App.Screens.Books;
using App.Screens.Catalog;
using App.Screens.AddCharacter;
using App.Screens.Home;
using App.Screens.ViewBookDetails;
using App.Screens.ViewCharacterDetailsScreen;
using Microsoft.Extensions.DependencyInjection;

namespace App.Configuration;

public static class Screens
{
    public static IServiceCollection AddScreens(this IServiceCollection services)
    {
        // would be awesome to just scan and get all ISCreen...need to think about this

        services.AddScoped<IHomeScreen, HomeScreen>()
            .AddScoped<ICatalogScreen, CatalogScreen>()
            .AddScoped<IAddAuthorScreen, AddAuthorScreen>()
            .AddScoped<IAddBookScreen, AddBookScreen>()
            .AddScoped<IAddCharacterScreen, AddCharacterScreen>()
            .AddScoped<IBookDetailsScreen, BookDetailsScreen>()
            .AddScoped<IViewCharacterDetailsScreen, ViewCharacterDetailsScreen>();

        return services;
    }

    public static IServiceCollection AddNavigators(this IServiceCollection services)
    {
        return services.AddScoped<IHomeScreenNavigator, HomeScreenNavigator>()
            .AddScoped<ICatalogScreenNavigator, CatalogScreenNavigator>()
            .AddScoped<IAddAuthorNavigator, AddAuthorNavigator>()
            .AddScoped<IAddBookScreenNavigator, AddBookScreenNavigator>()
            .AddScoped<IAddCharacterScreenNavigator, AddCharacterScreenNavigator>()
            .AddScoped<IBookDetailsScreenNavigator, BookDetailsScreenNavigator>()
            .AddScoped<IViewCharacterDetailsScreenNavigator, ViewCharacterDetailsScreenNavigator>();
    }
}
