using Data.CRUD.Create;
using Database;
using Database.Entites;

namespace Data.CRUD.Repositories;

public interface ICharacterRepository : IRepository<Character>
{
}

public class CharacterRepository : Repository<Character>, ICharacterRepository
{
    public CharacterRepository(AppDbContext context) : base(context)
    {
    }
}
