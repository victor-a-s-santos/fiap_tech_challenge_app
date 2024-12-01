using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class UpdateCustomerCommand : CustomerCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new UpdateCustomerValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
