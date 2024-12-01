using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class RemoveOrderValidation : OrderValidation<RemoveOrderCommand>
	{
		public RemoveOrderValidation()
		{
            ValidateOrderId();
        }
	}
}
