using Project_E_commerse.Repositories.Interfaces;
using Project_E_commerse.Models;
namespace Project_E_commerse.Services.Admin
{
    public interface IAdminService : IRepository<Project_E_commerse.Models.Address>
    {
        // Products
        Task<(IEnumerable<Project_E_commerse.Models.Product>? products, string msg)> GetAllProductsAsync();
        Task<(Project_E_commerse.Models.Product? product, string msg)> GetProductByIdAsync(int productId);
        Task<(Project_E_commerse.Models.Product? product, string msg)> CreateProductAsync(Project_E_commerse.Models.Product product);
        Task<(Project_E_commerse.Models.Product? product, string msg)> UpdateProductAsync(Project_E_commerse.Models.Product product);
        Task<string> DeleteProductAsync(int productId);

        // Orders
        Task<(IEnumerable<Project_E_commerse.Models.Order>? orders, string msg)> GetAllOrdersAsync();
        Task<string> UpdateOrderStatusAsync(int orderId, string status);
    }
}