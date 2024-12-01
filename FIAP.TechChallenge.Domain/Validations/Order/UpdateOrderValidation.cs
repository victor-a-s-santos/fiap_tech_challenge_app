using System;
using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
	public class UpdateOrderValidation : OrderValidation<UpdateOrderCommand>
    {
		public UpdateOrderValidation()
		{
			ValidateOrderId();
			ValidateOrder();
        }
	}
}

