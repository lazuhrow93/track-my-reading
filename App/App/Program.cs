using App.FirstScreen;
using Data.Queries.Data;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App;

public class Program
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

        IConfiguration config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        services.AddSingleton(config);

        services.AddSingleton<Menu>()
            .AddScoped<IBookQueries, BookQueries>()
            .AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("Database"));
            });

        return services.BuildServiceProvider();
    }
}
