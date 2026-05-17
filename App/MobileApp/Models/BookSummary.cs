namespace MobileApp.Models;

public enum ReadingState
{
    NotStarted,
    InProgress,
    Completed
}

public record BookSummary(
    int Id,
    string Title,
    string Author,
    ReadingState ReadingState,
    decimal Percentage);
