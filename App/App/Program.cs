using App.Configuration;
using App.Screens.Home;
using Data.Configuration;
using Data.CRUD.Read;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var provider = SetupDI();
        var menu = provider.GetRequiredService<IHomeScreen>();

        await menu.Show();
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
