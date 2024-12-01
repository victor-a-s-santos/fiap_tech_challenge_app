using FIAP.TechChallenge.Domain.Queries;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Queries.Order;

public class GetPagedOrdersQueryTest
{
    [Fact]
    public void IsValid_Should_ReturnTrue_When_Called()
    {
        // Arrange
        var query = new GetPagedOrdersQuery();

        // Act
        var result = query.IsValid();

        // Assert
        result.Should().BeTrue();
    }
}