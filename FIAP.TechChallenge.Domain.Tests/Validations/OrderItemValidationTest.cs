using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Validations;
using FluentValidation.TestHelper;

namespace FIAP.TechChallenge.Domain.Tests.Validations;

public class OrderItemValidationTest
{
    private readonly OrderItemValidation _validator;

    public OrderItemValidationTest()
    {
        _validator = new OrderItemValidation();
        _validator.ValidateOrderItem();
    }

    [Fact]
    public void ValidateOrderItem_Should_HaveValidationError_When_QuantityIsNull()
    {
        // Arrange
        var orderItem = new OrderItemCommand { Quantity = null };

        // Act
        var result = _validator.TestValidate(orderItem);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Quantity)
            .WithErrorMessage("A quantidade de itens é obrigatória");
    }

    [Fact]
    public void ValidateOrderItem_Should_HaveValidationError_When_ProductIdIsNull()
    {
        // Arrange
        var orderItem = new OrderItemCommand { ProductId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(orderItem);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ProductId)
            .WithErrorMessage("O id do produto é obrigatório");
    }
}
