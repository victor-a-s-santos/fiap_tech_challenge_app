using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Validations;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace FIAP.TechChallenge.Domain.Tests.Validations;

public class ProductValidationTest
{
    private class TestProductCommand : ProductCommand
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
    
    private readonly ProductValidation<TestProductCommand> _validator;

    public ProductValidationTest()
    {
        _validator = new ProductValidation<TestProductCommand>();
        _validator.ValidateProductId();
        _validator.ValidateProduct();
    }

    [Fact]
    public void ValidateProductId_ShouldHaveError_WhenIdIsNullOrEmpty()
    {
        // Arrange
        var command = new TestProductCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("O código do produto é obrigatório");
    }

    [Fact]
    public void ValidateProduct_ShouldHaveError_WhenNameIsNull()
    {
        // Arrange
        var command = new TestProductCommand { Name = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("O nome do produto é obrigatório");
    }

    [Fact]
    public void ValidateProduct_ShouldHaveError_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new TestProductCommand { Name = new string('A', 201) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("O nome do produto deve conter no máximo 200 caracteres");
    }

    [Fact]
    public void ValidateProduct_ShouldHaveError_WhenDescriptionIsNull()
    {
        // Arrange
        var command = new TestProductCommand { Description = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("A descrição do produto é obrigatória");
    }

    [Fact]
    public void ValidateProduct_ShouldHaveError_WhenDescriptionExceedsMaxLength()
    {
        // Arrange
        var command = new TestProductCommand { Description = new string('A', 501) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("A descrição do produto deve conter no máximo 500 caracteres");
    }

    [Fact]
    public void ValidateProduct_ShouldHaveError_WhenPriceIsNull()
    {
        // Arrange
        var command = new TestProductCommand { Price = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
            .WithErrorMessage("O preço do produto é obrigatório");
    }

    [Theory]
    [InlineData("InvalidCategory")]
    [InlineData(null)]
    public void ValidateProduct_ShouldHaveError_WhenCategoryIsInvalid(string invalidCategory)
    {
        // Arrange
        var command = new TestProductCommand { Category = invalidCategory };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category)
            .WithErrorMessage("A categoria informada é inválida");
    }
}
