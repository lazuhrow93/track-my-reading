using App.Screens.Catalog;
using App.Screens.Home;
using Microsoft.Extensions.DependencyInjection;

namespace App.Configuration;

public static class Screens
{
    public static IServiceCollection AddScreens(this IServiceCollection services)
    {
        // would be awesome to just scan and get all ISCreen...need to think about this

        services.AddScoped<IHomeScreen, HomeScreen>()
            .AddScoped<ICatalogScreen, CatalogScreen>();

        return services;
    }

    public static IServiceCollection AddNavigator(this IServiceCollection services)
    {
        return services.AddScoped<IHomeScreenNavigator, HomeScreenNavigator>();
    }
}
