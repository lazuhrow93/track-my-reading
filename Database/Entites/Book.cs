namespace Database.Entites;

public class Book : Entity
{
    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public int ReadingStatusId { get; set; }

    public Author? Author { get; set; }

    public ReadingStatus? ReadingStatus { get; set; }
}
