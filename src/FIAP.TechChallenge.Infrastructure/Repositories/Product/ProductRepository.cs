using FIAP.Crosscutting.Infrastructure.Repositories;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Infrastructure.Contexts;

namespace FIAP.TechChallenge.Infrastructure.Repositories
{
    public class ProductRepository : SqlRepository<Product>, IProductRepository
    {
        public ProductRepository(SqlContext context) : base(context) { }
    }
}
