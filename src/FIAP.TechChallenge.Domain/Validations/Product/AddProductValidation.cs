using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class AddProductValidation : ProductValidation<AddProductCommand>
    {
        public AddProductValidation()
        {
            ValidateProduct();
        }
    }
}
