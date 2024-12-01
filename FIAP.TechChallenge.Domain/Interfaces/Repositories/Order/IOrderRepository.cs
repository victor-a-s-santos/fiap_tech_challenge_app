using FIAP.Crosscutting.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Entities;

namespace FIAP.TechChallenge.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : ISqlRepository<Order>
    {
        Task<List<OrderDto>> GetOrders();
        Task<Order> GetOrderItemsById(Guid id);
    }
}
