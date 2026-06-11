using Api.Models;
using Data.CRUD.Read;
using Data.Services;

namespace Api.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        app.EditCharacterEndpoint();
        app.MapFetch();
        return app;
    }

    private static IEndpointRouteBuilder MapFetch(this IEndpointRouteBuilder app)
    {
        app.MapGet("/books", async (IBookQueries bookQueries, CancellationToken cancellationToken) =>
        {
            var books = await bookQueries.FetchAllWithAuthorAndStatus(cancellationToken);

            var result = books.Select(b => new BookSummary(
                b.Id,
                b.Title,
                b.Author!.Name,
                b.ReadingStatus!.State,
                b.ReadingStatus!.Percentage));

            return Results.Ok(result);
        });

        app.MapGet("book/{id}/characters", async (int id, ICharacterQueries characters, CancellationToken cancellationToken) =>
        {
            var chars = await characters.ByBookId(id, cancellationToken);

            var results = chars.Select(c => new CharacterSummary()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description ?? string.Empty
            });

            return Results.Ok(results);
        });

        app.MapPost("book/character", async (AddCharacterModel model, IAddService addService, CancellationToken cancellationToken) =>
        {
            var success = await addService.AddCharacter(model.Name, model.Description, model.BookId, cancellationToken);
            if (!success)
            {
                return Results.BadRequest();
            }
            return Results.Ok();
        });

        return app;
    }

    private static IEndpointRouteBuilder EditCharacterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("characters/{id}", async (int id, EditCharacterModel model, IEditService editService, CancellationToken cancellationToken) =>
        {
            var success = await editService.EditCharacter(id, model.Name, cancellationToken);

            if (!success)
            {
                return Results.BadRequest();
            }
            return Results.Ok();

        });

        return app;
    }
}
