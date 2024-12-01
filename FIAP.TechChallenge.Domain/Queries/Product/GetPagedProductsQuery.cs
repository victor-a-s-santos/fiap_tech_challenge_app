using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetPagedProductsQuery : ProductQuery<PagedResult<ProductDto>>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
