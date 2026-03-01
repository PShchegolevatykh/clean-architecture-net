using CleanArchitecture.Domain.Common;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Domain.Tests.Common;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void BaseEntity_ShouldGenerateV7Guid()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.Id.ShouldNotBe(Guid.Empty);
        entity.Id.Version.ShouldBe(7);
    }

    [Fact]
    public void BaseEntity_ShouldSetCreatedAt()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
