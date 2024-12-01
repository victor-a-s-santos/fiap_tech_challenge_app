using FIAP.TechChallenge.Domain.Commands;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Commands.Order;

public class RemoveOrderCommandTest
{
    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCommandIsValid()
    {
        // Arrange
        var command = new RemoveOrderCommand
        {
            Id = Guid.NewGuid(),
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
        var command = new RemoveOrderCommand
        {
            Id = Guid.Empty, // Id vazio
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
        var command = new RemoveOrderCommand
        {
            Id = Guid.Empty, // Id vazio
        };

        // Act
        command.IsValid();
        var errors = command.ValidationResult.Errors;

        // Assert
        errors.Should().Contain(e => e.ErrorMessage.Contains("O código do pedido é obrigatório"));
    }
}