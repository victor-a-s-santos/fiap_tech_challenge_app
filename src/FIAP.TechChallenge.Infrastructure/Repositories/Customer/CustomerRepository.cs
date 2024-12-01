using FIAP.Crosscutting.Infrastructure.Repositories;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Infrastructure.Contexts;

namespace FIAP.TechChallenge.Infrastructure.Repositories
{
    public class CustomerRepository : SqlRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SqlContext context) : base(context) { }
    }
}
