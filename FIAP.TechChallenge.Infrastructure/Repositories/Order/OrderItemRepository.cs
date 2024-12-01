using FIAP.Crosscutting.Infrastructure.Repositories;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Infrastructure.Contexts;

namespace FIAP.TechChallenge.Infrastructure.Repositories.Order
{
    public class OrderItemRepository : SqlRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(SqlContext context) : base(context) { }
    }
}

