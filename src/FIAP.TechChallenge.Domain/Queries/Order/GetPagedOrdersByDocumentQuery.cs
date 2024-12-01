using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetPagedOrdersByDocumentQuery : OrderQuery<PagedResult<OrderDto>>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}

