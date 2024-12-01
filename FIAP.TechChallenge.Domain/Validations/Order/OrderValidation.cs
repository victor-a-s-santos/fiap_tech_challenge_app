using FIAP.TechChallenge.Domain.Commands;
using FluentValidation;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class OrderValidation<TCommand> : AbstractValidator<TCommand> where TCommand : OrderCommand
    {
        public void ValidateOrderId()
        {
            RuleFor(x => x.Id)
                .NotNull().NotEqual(Guid.Empty).WithMessage("O código do pedido é obrigatório");
        }

        public void ValidateOrder()
        {
            RuleFor(x => x.Total)
                .NotNull().WithMessage("O total do pedido é obrigatório");

            RuleForEach(x => x.OrderItems).SetInheritanceValidator(v =>
            {
                v.Add(new OrderItemValidation());
            });
        }
    }

    public class OrderItemValidation : AbstractValidator<OrderItemCommand>
    {
        public void ValidateOrderItem()
        {
            RuleFor(x => x.Quantity)
                .NotNull().WithMessage("A quantidade de itens é obrigatória");

            RuleFor(x => x.ProductId)
                .NotNull().NotEqual(Guid.Empty).WithMessage("O id do produto é obrigatório");
        }
    }
}