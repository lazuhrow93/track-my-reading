namespace Database.Entites;

public class Trait : Entity
{
    public string Description { get; set; } = null!;

    public List<Character>? Characters { get; set; }
}
