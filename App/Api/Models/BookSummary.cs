using Database.Entites;

namespace Api.Models;

public record BookSummary(
    int Id,
    string Title,
    string Author,
    ReadingState ReadingState,
    decimal Percentage);
