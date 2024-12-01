using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetProductsByCategoryQuery : ProductQuery<List<ProductDto>>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
