using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.TechChallenge.Application.ViewModels;

namespace FIAP.TechChallenge.Application.Interfaces
{
    public interface IOrderServiceApp
	{
        Task<List<OrderResponseViewModel>> GetOrders();
        Task<PagedResult<OrderResponseViewModel>> GetPagedOrders(int page, int take, string orderProperty, bool orderDesc);
        Task<OrderResponseViewModel> GetOrder(string id);
        Task<PagedResult<OrderResponseViewModel>> GetPagedOrdersByCustomerId(string customerId, int page, int take, string orderProperty, bool orderDesc);
        Task SaveOrder(OrderRequestViewModel viewModel, bool update = false);
        Task RemoveOrder(string id);
    }
}