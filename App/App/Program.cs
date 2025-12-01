using App.FirstScreen;
using Data.Queries.Data;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace App;
public static class Program
{
    public static void Main(string[] args)
    {
        var provider = SetupDI();
        var menu = provider.GetRequiredService<Menu>();

        menu.Start();
    }

    private static ServiceProvider SetupDI()
    {
        var services = new ServiceCollection();
        services.AddSingleton<Menu>()
            .AddScoped<IBookQueries, BookQueries>();

        return services.BuildServiceProvider();
    }
}
