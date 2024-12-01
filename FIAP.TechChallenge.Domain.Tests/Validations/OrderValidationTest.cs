using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Validations;
using FluentValidation.TestHelper;

namespace FIAP.TechChallenge.Domain.Tests.Validations;

public class OrderValidationTest
{
    private class TestOrderCommand: OrderCommand
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
    
    private readonly OrderValidation<TestOrderCommand> _validator;

    public OrderValidationTest()
    {
        _validator = new OrderValidation<TestOrderCommand>();
        _validator.ValidateOrderId();
        _validator.ValidateOrder();
    }

    [Fact]
    public void ValidateOrderId_Should_HaveValidationError_When_IdIsEmpty()
    {
        // Arrange
        var command = new TestOrderCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("O código do pedido é obrigatório");
    }

    [Fact]
    public void ValidateOrder_Should_HaveValidationError_When_TotalIsNull()
    {
        // Arrange
        var command = new TestOrderCommand { Total = null };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Total)
            .WithErrorMessage("O total do pedido é obrigatório");
    }
}
