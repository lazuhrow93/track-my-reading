using Data.CRUD.Read;
using Data.CRUD.Repositories;

namespace Data.Services;

public interface IEditService
{
    Task<bool> EditCharacter(int id, string name, CancellationToken cancellationToken);
    Task<bool> EditNote(int id, string value, CancellationToken cancellationToken);
    Task<bool> EditTrait(int id, string name, CancellationToken cancellationToken);
}

public class EditService : IEditService
{
    private readonly ICharacterQueries _characterQueries;
    private readonly ICharacterRepository _characterRepository;

    public EditService(ICharacterQueries characterQueries, ICharacterRepository characterRepository)
    {
        _characterQueries = characterQueries;
        _characterRepository = characterRepository;
    }

    public async Task<bool> EditCharacter(int id, string name, CancellationToken cancellationToken)
    {
        var character = await _characterQueries.GetById(id, cancellationToken);

        if (character == null)
        {
            return false;
        }

        character.Name = name;
        return await _characterRepository.SaveChanges(cancellationToken) == 1; //since we updated one thing.
        
    }

    public Task<bool> EditNote(int id, string value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EditTrait(int id, string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
