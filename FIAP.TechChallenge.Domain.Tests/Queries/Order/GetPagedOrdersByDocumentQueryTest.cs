using FIAP.TechChallenge.Domain.Queries;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Queries.Order;

public class GetPagedOrdersByDocumentQueryTest
{
    [Fact]
    public void IsValid_Should_ReturnTrue_When_Called()
    {
        // Arrange
        var query = new GetPagedOrdersByDocumentQuery();

        // Act
        var result = query.IsValid();

        // Assert
        result.Should().BeTrue();
    }
}