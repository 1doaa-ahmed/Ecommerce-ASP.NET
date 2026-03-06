using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_E_commerse.Data;
using Project_E_commerse.Models;
using Project_E_commerse.Repositories;

namespace Project_E_commerse.Services.Order
{
    public class OrderService : Repository<Project_E_commerse.Models.Order>, IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;



        public OrderService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager; 
        }


        public async Task<IEnumerable<Project_E_commerse.Models.Order>> GetByUserAsync(string userId)
            => await _dbSet
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<Project_E_commerse.Models.Order?> GetWithDetailsAsync(int orderId)
            => await _dbSet
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

        public async Task<IEnumerable<Project_E_commerse.Models.Order>> GetAllWithDetailsAsync()
            => await _dbSet
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<(Project_E_commerse.Models.Cart? cart, string msg)> GetCartByUserAsync(string userId)
        {
            // 1. Check user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (null, "User not found!");
            // 2. Get cart with all details
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return (null, "Cart is empty!");

            return (cart, $"Cart has {cart.CartItems.Count} items");
        }

        public async Task<(IEnumerable<Project_E_commerse.Models.Order>? orders, string msg)> GetUserOrdersAsync(string userId)
        {
            // 1. Check user exists
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Include(o => o.ShippingAddress)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            if (!orders.Any())
                return (null, "You have no orders yet!");

            return (orders, $"You have {orders.Count} orders");
        }
        public async Task<(Project_E_commerse.Models.Order? order, string msg)> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                            .Include(o => o.OrderItems)
                                .ThenInclude(i => i.Product)
                            .Include(o => o.ShippingAddress)
                            .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return (null, "Order not found!");

            return (order, "Order found ✅");
        }

    }
}