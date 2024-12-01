using FIAP.TechChallenge.Domain.Commands;
using FluentAssertions;

namespace FIAP.TechChallenge.Domain.Tests.Commands.Customer;

public class UpdateCustomerCommandTest
{
    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateCustomerCommand
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Document = "09191043042",
            Email = "email@email.com"
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
        var command = new UpdateCustomerCommand
        {
            Id = Guid.Empty,
            Name = "", // Nome vazio
            Document = "123", // Documento inválido
            Email = "invalid-email", // Email inválido
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
        var command = new UpdateCustomerCommand
        {
            Id = Guid.Empty, // Id vazio
            Name = "", // Nome vazio
            Document = "123", // Documento inválido
            Email = "invalid-email", // Email inválido
        };

        // Act
        command.IsValid();
        var errors = command.ValidationResult.Errors;

        // Assert
        errors.Should().Contain(e => e.ErrorMessage.Contains("O código do cliente é obrigatório"));
        errors.Should().Contain(e => e.ErrorMessage.Contains("O nome do cliente é obrigatório"));
        errors.Should().Contain(e => e.ErrorMessage.Contains("O CPF informado é inválido"));
        errors.Should().Contain(e => e.ErrorMessage.Contains("O e-mail informado é inválido"));
    }
}