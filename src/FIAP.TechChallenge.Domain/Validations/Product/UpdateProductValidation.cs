using FIAP.TechChallenge.Domain.Commands;

namespace FIAP.TechChallenge.Domain.Validations
{
    public class UpdateProductValidation : ProductValidation<UpdateProductCommand>
    {
        public UpdateProductValidation()
        {
            ValidateProductId();
            ValidateProduct();
        }
    }
}
