using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class RemoveProductValidation : ProductValidation<RemoveProductCommand>
    {
        public RemoveProductValidation()
        {
            ValidateProductId();
        }
    }
}
