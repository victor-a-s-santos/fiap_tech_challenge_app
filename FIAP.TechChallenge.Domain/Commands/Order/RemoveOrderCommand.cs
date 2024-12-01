using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
    public class RemoveOrderCommand : OrderCommand
	{
        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}

