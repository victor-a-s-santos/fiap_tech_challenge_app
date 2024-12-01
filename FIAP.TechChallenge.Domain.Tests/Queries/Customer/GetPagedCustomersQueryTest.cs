using FIAP.TechChallenge.Domain.Queries;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Queries.Customer;

public class GetPagedCustomersQueryTest
{
    [Fact]
    public void IsValid_Should_ReturnTrue_When_Called()
    {
        // Arrange
        var query = new GetCustomerByDocumentQuery();

        // Act
        var result = query.IsValid();

        // Assert
        result.Should().BeTrue();
    }
}