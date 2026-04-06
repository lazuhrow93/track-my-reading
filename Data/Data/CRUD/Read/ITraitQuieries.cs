using Database;
using Database.Entites;
using Microsoft.EntityFrameworkCore;

namespace Data.CRUD.Read;

public interface ITraitQuieries : IEntityQueries<Trait>
{
    Task<List<Trait>> GetAll(CancellationToken cancellation);
}

public class TraitQueries : EntityQueriesBase<Trait>, ITraitQuieries
{
    public TraitQueries(AppDbContext context) : base(context)
    {
    }

    public Task<List<Trait>> GetAll(CancellationToken cancellation)
    {
        return Query.ToListAsync(cancellation);
    }
}
