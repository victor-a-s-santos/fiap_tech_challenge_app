using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Enumerators;
using FluentValidation;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class ProductValidation<TCommand> : AbstractValidator<TCommand> where TCommand : ProductCommand
    {
        public void ValidateProductId()
        {
            RuleFor(x => x.Id)
                .NotNull().NotEqual(Guid.Empty).WithMessage("O código do produto é obrigatório");
        }

        public void ValidateProduct()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("O nome do produto é obrigatório")
                .MaximumLength(200).WithMessage("O nome do produto deve conter no máximo 200 caracteres");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("A descrição do produto é obrigatória")
                .MaximumLength(500).WithMessage("A descrição do produto deve conter no máximo 500 caracteres");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("O preço do produto é obrigatório");

            RuleFor(x => x.Category)
                .Must(ValidateCategory).WithMessage("A categoria informada é inválida");
        }

        private bool ValidateCategory(string category)
        {
            return category == EnumExtension.GetDescription(ProductCategoryEnum.Sandwich)
                || category == EnumExtension.GetDescription(ProductCategoryEnum.Accompaniment)
                || category == EnumExtension.GetDescription(ProductCategoryEnum.Beverage)
                || category == EnumExtension.GetDescription(ProductCategoryEnum.Dessert);
        }
    }
}
