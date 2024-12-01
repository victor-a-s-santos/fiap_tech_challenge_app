using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetCustomerQuery : CustomerQuery<CustomerDto>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
