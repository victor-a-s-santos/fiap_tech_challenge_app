using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetOrderQuery : OrderQuery<OrderDto>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}

