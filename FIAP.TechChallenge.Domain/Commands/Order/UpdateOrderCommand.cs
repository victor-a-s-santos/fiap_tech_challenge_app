using System;
using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
	public class UpdateOrderCommand : OrderCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}

