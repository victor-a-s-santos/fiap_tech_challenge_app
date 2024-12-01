using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetPagedCustomersQuery : CustomerQuery<PagedResult<CustomerDto>>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
