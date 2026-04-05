namespace Database.Entites;

public class Character : Entity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public int BookId { get; set; }

    public Book? Book { get; set; }

    public List<Trait> Traits { get; set; } = [];

    public List<Note> Notes { get; set; } = [];
}
