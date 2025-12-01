using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Convert table name to snake_case
            var tableName = ToSnakeCase(entity.GetTableName()!);
            entity.SetTableName(tableName);

            // Convert column names to snake_case (optional)
            foreach (var property in entity.GetProperties())
            {
                var columnName = ToSnakeCase(property.GetColumnName());
                property.SetColumnName(columnName);
            }

            // Convert key/foreign key names too (optional)
            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
            }
        }
    }

    private static string? ToSnakeCase(string? name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        return Regex.Replace(name, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
