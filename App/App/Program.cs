using App.Configuration;
using App.Screens.Home;
using Data.Configuration;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace App;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        var provider = SetupDI();
        var menu = provider.GetRequiredService<IHomeScreen>();
        AnsiConsole.Background = Color.Grey11;
        await menu.Show(HomeScreenInput.Default, cts.Token);
    }

    private static ServiceProvider SetupDI()
    {
        var services = new ServiceCollection();

        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        services.AddSingleton(config);

        services.AddScreens()
            .AddNavigators()
            .AddRepositories()
            .AddQueries()
            .AddServices()
            .AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("Database"));
            });

        return services.BuildServiceProvider();
    }
}
