using Data.CRUD.Create;
using Data.CRUD.Read;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.AddScoped<IBookQueries, BookQueries>()
            .AddScoped<IAuthorQueries, AuthorQueries>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
