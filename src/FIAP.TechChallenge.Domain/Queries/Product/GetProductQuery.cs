using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetProductQuery : ProductQuery<ProductDto>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
