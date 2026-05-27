namespace MobileApp.Models;

public class AddCharacterModel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int BookId { get; set; }
}
