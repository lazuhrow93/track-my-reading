using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Database;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Build configuration from App project (where user secrets live)
        var appProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "../App");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(appProjectPath)
            .AddUserSecrets("67d6504c-1494-42c0-bbeb-c0e4249fc824") // load user secrets
            .Build();

        // Get connection string
        var connectionString = configuration.GetConnectionString("Database");

        // Configure DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}