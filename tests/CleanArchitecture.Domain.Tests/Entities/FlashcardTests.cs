using CleanArchitecture.Domain.Entities;
using Shouldly;

namespace CleanArchitecture.Domain.Tests.Entities;

public class FlashcardTests
{
    [Fact]
    public void Create_Should_SetPropertiesCorrectly_When_ValidData()
    {
        // Arrange
        var front = "Front content";
        var back = "Back content";
        var description = "Optional description";
        var difficulty = 3;

        // Act
        var flashcard = Flashcard.Create(front, back, description, difficulty);

        // Assert
        flashcard.Front.ShouldBe(front);
        flashcard.Back.ShouldBe(back);
        flashcard.Description.ShouldBe(description);
        flashcard.Difficulty.ShouldBe(difficulty);
    }

    [Theory]
    [InlineData("", "Back")]
    [InlineData(" ", "Back")]
    [InlineData(null, "Back")]
    public void Create_Should_ThrowArgumentException_When_FrontIsEmpty(string front, string back)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Flashcard.Create(front!, back, null, 1));
    }

    [Theory]
    [InlineData("Front", "")]
    [InlineData("Front", " ")]
    [InlineData("Front", null)]
    public void Create_Should_ThrowArgumentException_When_BackIsEmpty(string front, string back)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => Flashcard.Create(front, back!, null, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Create_Should_ThrowArgumentOutOfRangeException_When_DifficultyIsInvalid(int difficulty)
    {
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => Flashcard.Create("Front", "Back", null, difficulty));
    }
}