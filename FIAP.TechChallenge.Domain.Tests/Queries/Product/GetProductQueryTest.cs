using FIAP.TechChallenge.Domain.Queries;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Queries.Product;

public class GetProductQueryTest
{
    [Fact]
    public void IsValid_Should_ReturnTrue_When_Called()
    {
        // Arrange
        var query = new GetProductQuery();

        // Act
        var result = query.IsValid();

        // Assert
        result.Should().BeTrue();
    }
}