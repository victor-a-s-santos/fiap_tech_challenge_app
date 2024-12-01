using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class AddCustomerCommand : CustomerCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new AddCustomerValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}
