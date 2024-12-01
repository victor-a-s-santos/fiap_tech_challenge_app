using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class UpdateCustomerValidation : CustomerValidation<UpdateCustomerCommand>
    {
        public UpdateCustomerValidation()
        {
            ValidateCustomerId();
            ValidateCustomer();
        }
    }
}
