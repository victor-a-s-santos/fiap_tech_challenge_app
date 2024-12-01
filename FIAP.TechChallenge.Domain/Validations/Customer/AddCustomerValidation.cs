using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class AddCustomerValidation : CustomerValidation<AddCustomerCommand>
    {
        public AddCustomerValidation()
        {
            ValidateCustomer();
        }
    }
}
