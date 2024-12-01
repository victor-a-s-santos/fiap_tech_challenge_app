using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class RemoveCustomerCommand : CustomerCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new RemoveCustomerValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
