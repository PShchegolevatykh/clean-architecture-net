using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain.Entities;

public class Flashcard : BaseEntity
{
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Difficulty { get; set; } // 1-5
}
