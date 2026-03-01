using CleanArchitecture.Domain.Common;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Domain.Tests.Common;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void Id_Should_BeV7Guid_When_EntityIsCreated()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.Id.ShouldNotBe(Guid.Empty);
        entity.Id.Version.ShouldBe(7);
    }

    [Fact]
    public void CreatedAt_Should_BeSet_When_EntityIsCreated()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
