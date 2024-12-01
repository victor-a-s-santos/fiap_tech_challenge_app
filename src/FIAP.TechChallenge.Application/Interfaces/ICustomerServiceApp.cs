using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.TechChallenge.Application.ViewModels;

namespace FIAP.TechChallenge.Application.Interfaces
{
    public interface ICustomerServiceApp
    {
        Task<PagedResult<CustomerResponseViewModel>> GetPagedCustomers(int page, int take, string orderProperty, bool orderDesc);
        Task<CustomerResponseViewModel> GetCustomer(string id);
        Task<CustomerResponseViewModel> GetCustomerByDocument(string document);
        Task SaveCustomer(CustomerRequestViewModel viewModel, bool update = false);
        Task RemoveCustomer(string id);
    }
}
