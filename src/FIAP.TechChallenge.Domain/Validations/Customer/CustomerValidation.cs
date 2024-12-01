using FIAP.Crosscutting.Domain.Helpers.Validators;
using FIAP.TechChallenge.Domain.Commands;
using FluentValidation;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class CustomerValidation<TCommand> : AbstractValidator<TCommand> where TCommand : CustomerCommand
    {
        public void ValidateCustomerId()
        {
            RuleFor(x => x.Id)
                .NotNull().NotEqual(Guid.Empty).WithMessage("O código do cliente é obrigatório");
        }

        public void ValidateCustomer()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty().WithMessage("O nome do cliente é obrigatório")
                .MaximumLength(150).WithMessage("O nome do cliente não pode conter mais de 150 caracteres");

            RuleFor(x => x.Document)
                .NotNull().WithMessage("O CPF do cliente é obrigatório")
                .MaximumLength(11).WithMessage("O CPF do cliente não pode conter mais de 11 caracteres")
                .Must(Validator.CPF).WithMessage("O CPF informado é inválido");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("O e-mail do cliente é obrigatório")
                .MaximumLength(250).WithMessage("O e-mail do cliente não pode conter mais de 250 caracteres")
                .EmailAddress().WithMessage("O e-mail informado é inválido");
        }
    }
}
