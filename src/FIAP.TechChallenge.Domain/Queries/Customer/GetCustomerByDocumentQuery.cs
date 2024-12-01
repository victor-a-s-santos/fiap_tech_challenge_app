using FIAP.TechChallenge.Domain.DataTransferObjects;

namespace FIAP.TechChallenge.Domain.Queries
{
    public class GetCustomerByDocumentQuery : CustomerQuery<CustomerDto>
    {
        public override bool IsValid()
        {
            return true;
        }
    }
}
