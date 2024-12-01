using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetOrdersQuery : OrderQuery<List<OrderDto>>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
