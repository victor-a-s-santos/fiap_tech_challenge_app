using System;
using FIAP.TechChallenge.Domain.Validations;

namespace FIAP.TechChallenge.Domain.Commands
{
	public class AddOrderCommand : OrderCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new AddOrderValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }
}

