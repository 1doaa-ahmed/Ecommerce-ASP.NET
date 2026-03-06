using Project_E_commerse.Models;
using Project_E_commerse.Repositories.Interfaces;

namespace Project_E_commerse.Services.Order
{
    public interface IOrderService : IRepository<Project_E_commerse.Models.Order>
    {
        Task<IEnumerable<Project_E_commerse.Models.Order>> GetByUserAsync(string userId);
        Task<Project_E_commerse.Models.Order?> GetWithDetailsAsync(int orderId);
        Task<IEnumerable<Project_E_commerse.Models.Order>> GetAllWithDetailsAsync();
        Task<(Project_E_commerse.Models.Cart? cart, string msg)> GetCartByUserAsync(string userId);
        Task<(IEnumerable<Project_E_commerse.Models.Order>? orders, string msg)> GetUserOrdersAsync(string userId);
        Task<(Project_E_commerse.Models.Order? order, string msg)> GetOrderByIdAsync(int orderId);
    }
}