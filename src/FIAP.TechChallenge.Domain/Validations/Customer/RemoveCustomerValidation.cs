using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class RemoveCustomerValidation : CustomerValidation<RemoveCustomerCommand>
    {
        public RemoveCustomerValidation()
        {
            ValidateCustomerId();
        }
    }
}
