using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class RemoveProductCommand : ProductCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new RemoveProductValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
