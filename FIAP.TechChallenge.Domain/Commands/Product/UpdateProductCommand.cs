using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class UpdateProductCommand : ProductCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new UpdateProductValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
