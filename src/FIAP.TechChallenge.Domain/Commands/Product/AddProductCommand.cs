using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class AddProductCommand : ProductCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new AddProductValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
