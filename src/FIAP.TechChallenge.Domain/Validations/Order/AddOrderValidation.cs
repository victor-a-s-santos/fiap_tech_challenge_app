using System;
using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
	public class AddOrderValidation : OrderValidation<AddOrderCommand>
    {
        public AddOrderValidation()
        {
            ValidateOrder();
        }
    }
}

