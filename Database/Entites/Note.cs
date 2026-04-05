namespace Database.Entites;

public class Note : Entity
{
    public int CharacterId { get; set; }

    public string Value { get; set; } = null!;

    public Character? Character { get; set; }
}