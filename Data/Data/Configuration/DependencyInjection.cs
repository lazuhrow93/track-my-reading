using Data.CRUD.Create;
using Data.CRUD.Read;
using Data.CRUD.Repositories;
using Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        return services.AddScoped<IBookQueries, BookQueries>()
            .AddScoped<IAuthorQueries, AuthorQueries>()
            .AddScoped<ICharacterQueries, CharacterQueries>()
            .AddScoped<ITraitQuieries, TraitQueries>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<ICharacterRepository, CharacterRepository>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddScoped<IAddService, AddService>()
            .AddScoped<IEditService, EditService>();
    }
}
