using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Features.Flashcards.Commands.CreateFlashcard;
using CleanArchitecture.Domain.Entities;
using Shouldly;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CleanArchitecture.Application.Tests.Features.Flashcards.Commands.CreateFlashcard;

public class CreateFlashcardHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock = new Mock<IApplicationDbContext>();
    private readonly CreateFlashcardHandler _handler;

    public CreateFlashcardHandlerTests()
    {
        // Mocking DbSet is tricky with Moq, but for simple Add it works if we setup the DbSet
        var flashcards = new List<Flashcard>();
        var mockSet = CreateMockDbSet(flashcards);
        _contextMock.Setup(x => x.Flashcards).Returns(mockSet.Object);
        
        _handler = new CreateFlashcardHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateFlashcardAndReturnId()
    {
        // Arrange
        var command = new CreateFlashcardCommand
        {
            Front = "Front",
            Back = "Back",
            Difficulty = 3
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.ShouldNotBe(Guid.Empty);
        _contextMock.Verify(x => x.Flashcards.Add(It.IsAny<Flashcard>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return mockSet;
    }
}
