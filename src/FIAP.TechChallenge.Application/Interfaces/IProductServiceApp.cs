using FIAP.TechChallenge.Application.ViewModels;

namespace FIAP.TechChallenge.Application.Interfaces
{
    public interface IProductServiceApp
    {
        Task<List<ProductViewModel>> GetProductsByCategory(string category);
        Task SaveProduct(ProductViewModel viewModel, bool update = false);
        Task RemoveProduct(string id);
    }
}
