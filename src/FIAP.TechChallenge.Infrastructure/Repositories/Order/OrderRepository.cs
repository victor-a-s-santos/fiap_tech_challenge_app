using FIAP.Crosscutting.Infrastructure.Repositories;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FIAP.TechChallenge.Infrastructure.Repositories
{
    public class OrderRepository : SqlRepository<Domain.Entities.Order>, IOrderRepository
    {
        private readonly SqlContext _context;

        public OrderRepository(SqlContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetOrders()
        {
            var query = await DbSet()
                .Include(x => x.Customer)
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    CustomerName = x.Customer != null ? x.Customer.Name : "Anônimo",
                    Total = x.Total,
                    CreatedAt = x.CreatedAt,
                    Situation = x.Situation,
                    OrderItems = _context.OrderItems
                        .Where(o => o.OrderId == x.Id)
                        .Include(o => o.Product)
                        .Select(o => new OrderItemDto
                        {
                            ProductId = o.ProductId,
                            ProductName = o.Product.Name,
                            Quantity = o.Quantity
                        }).ToList()
                }).ToListAsync();

            return query;
        }

        public async Task<Domain.Entities.Order> GetOrderItemsById(Guid id)
        {
            return await DbSet().Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
