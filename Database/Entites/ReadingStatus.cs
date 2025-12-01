namespace Database.Entites;

public enum ReadingState
{
    NotStarted,
    InProgress,
    Completed
}

public class ReadingStatus : Entity
{
    public ReadingState State { get; set; }

    public string Description { get; set; } = null!;

    public decimal Percentage { get; set; } = 0m;

    public int BookId { get; set; }
}
