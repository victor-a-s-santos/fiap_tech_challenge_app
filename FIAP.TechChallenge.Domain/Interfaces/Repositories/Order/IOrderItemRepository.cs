using FIAP.Crosscutting.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Entities;

namespace FIAP.TechChallenge.Domain.Interfaces.Repositories
{
    public interface IOrderItemRepository : ISqlRepository<OrderItem>
	{
	}
}

