using Project_E_commerse.Models;
using Project_E_commerse.Repositories.Interfaces;

namespace Project_E_commerse.Services.Cart
{
    public interface ICartService : IRepository<Project_E_commerse.Models.Product>
    {
        Task<(CartItem? item, string msg)> AddToCartAsync(string userId, int productId, int qty);
        Task<string> RemoveFromCartAsync(string userId, int cartItemId);
        Task<(CartItem? item, string msg)> UpdateQuantityAsync(string userId, int cartItemId, int newQty);
        Task<(Project_E_commerse.Models.Cart? cart, string msg)> GetCartByUserAsync(string userId);
        Task<(Project_E_commerse.Models.Order? order, string msg)> CheckoutAsync(string userId, int selectedAddressId);

    }
}