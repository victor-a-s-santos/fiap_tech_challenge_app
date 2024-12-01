using FIAP.TechChallenge.Domain.Commands;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Commands.Order;

public class UpdateOrderCommandTest
{
    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Total = 200,
            OrderItems = new List<OrderItemCommand>
            {
                new() { ProductId = Guid.NewGuid(), Quantity = 2 },
                new() { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        // Act
        var result = command.IsValid();

        // Assert
        result.Should().BeTrue();
        command.ValidationResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            CustomerId = Guid.Empty,
            Total = null,
            OrderItems = new List<OrderItemCommand>
            {
                new() { ProductId = Guid.Empty, Quantity = null },
                new() { ProductId = Guid.Empty, Quantity = null }
            }
        };
    
        // Act
        var result = command.IsValid();
    
        // Assert
        result.Should().BeFalse();
        command.ValidationResult.Errors.Should().NotBeEmpty();
    }
    
    [Fact]
    public void IsValid_ShouldContainCorrectErrorMessages_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            Id = Guid.Empty,
            CustomerId = Guid.Empty,
            Total = null,
            OrderItems = new List<OrderItemCommand>
            {
                new() { ProductId = Guid.Empty, Quantity = null },
                new() { ProductId = Guid.Empty, Quantity = null }
            }
        };
    
        // Act
        command.IsValid();
        var errors = command.ValidationResult.Errors;
    
        // Assert
        errors.Should().Contain(e => e.ErrorMessage.Contains("O código do pedido é obrigatório"));
        errors.Should().Contain(e => e.ErrorMessage.Contains("O total do pedido é obrigatório"));
    }
}