using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public class Flashcard : BaseEntity
{
    private Flashcard() { } // Required for EF Core

    public string Front { get; private set; } = string.Empty;
    public string Back { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Difficulty { get; private set; } // 1-5

    public static Flashcard Create(string front, string back, string? description, int difficulty)
    {
        if (string.IsNullOrWhiteSpace(front))
            throw new ArgumentException("Front cannot be empty", nameof(front));
        
        if (string.IsNullOrWhiteSpace(back))
            throw new ArgumentException("Back cannot be empty", nameof(back));

        if (difficulty is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(difficulty), "Difficulty must be between 1 and 5");

        return new Flashcard
        {
            Front = front,
            Back = back,
            Description = description,
            Difficulty = difficulty
        };
    }
}
