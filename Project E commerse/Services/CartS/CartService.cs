using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_E_commerse.Data;
using Project_E_commerse.Models;
using Project_E_commerse.Repositories;
using Project_E_commerse.Repositories.Interfaces;

namespace Project_E_commerse.Services.Cart
{
    public class CartService : Repository<Project_E_commerse.Models.Product>, ICartService
    {
        public CartService(ApplicationDbContext context) : base(context) { }

        // ==================== Add to Cart ====================
        public async Task<(CartItem? item, string msg)> AddToCartAsync(string userId, int productId, int qty)
        {
            // 1. Check product if exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return (null, "Product not found!");
            // 2. Check stock
            if (product.StockQuantity < qty)
                return (null, $"Not enough stock! Available: {product.StockQuantity}");
            // 3. Get or Create Cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Project_E_commerse.Models.Cart { UserId = userId };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            // 4. Check if product already in cart
            var existingItem = cart.CartItems?
                .FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                // just update quantity
                existingItem.Quantity += qty;
                existingItem.UnitPrice = product.Price;
            }
            else
            {
                // add new item
                existingItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = qty,
                    UnitPrice = product.Price
                };
                await _context.CartItems.AddAsync(existingItem);
            }

            await _context.SaveChangesAsync();
            return (existingItem, "Product added to cart successfully");
        }

        // ==================== Remove from Cart ====================
        public async Task<string> RemoveFromCartAsync(string userId, int cartItemId)
        {
            // 1. Get cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return "Cart not found!";
            // 2. Get item
            var item = cart.CartItems?
                .FirstOrDefault(i => i.CartItemId == cartItemId);
            if (item == null)
                return "Item not found in cart!";
            // 3. Remove
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return "Item removed successfully";
        }

        // Update Quantity
        public async Task<(CartItem? item, string msg)> UpdateQuantityAsync(string userId, int cartItemId, int newQty)
        {
            // 1. Get cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return (null, "Cart not found!");
            // 2. Get item
            var item = cart.CartItems?
                .FirstOrDefault(i => i.CartItemId == cartItemId);

            if (item == null)
                return (null, "Item not found in cart!");

            // 3. Check stock
            if (item.Product!.StockQuantity < newQty)
                return (null, $"Not enough stock! Available: {item.Product.StockQuantity}");

            // 4. Update
            item.Quantity = newQty;
            await _context.SaveChangesAsync();

            return (item, "Quantity updated successfully");
        }
        public async Task<(Project_E_commerse.Models.Cart? cart, string msg)> GetCartByUserAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(i => i.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return (null, "Cart is empty!");

            return (cart, $"Cart has {cart.CartItems.Count} items");
        }
        public async Task<(Project_E_commerse.Models.Order? order, string msg)> CheckoutAsync(string userId, int selectedAddressId)
        {
            // 1. Get cart
            var (cart, msg) = await GetCartByUserAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                return (null, "Your cart is empty!");
            // 2. Check address
            var address = await _context.Addresses
                                       .FirstOrDefaultAsync(a => a.AddressId == selectedAddressId && a.UserId == userId);
            if (address == null)
                return (null, "Invalid address!");
            // 3. Check stock for all items
            foreach (var item in cart.CartItems)
            {
                if (item.Product!.StockQuantity < item.Quantity)
                    return (null, $"Not enough stock for {item.Product.Name}! Available: {item.Product.StockQuantity}");
            }

            // 4. Create Order
            var order = new Project_E_commerse.Models.Order
            {
                OrderNumber = Guid.NewGuid().ToString("N")[..10].ToUpper(),
                OrderDate = DateTime.Now,
                UserId = userId,
                ShippingAddressId = address.AddressId,
                TotalAmount = cart.CartItems.Sum(i => i.LineTotal),
                OrderItems = cart.CartItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.LineTotal
                }).ToList()
            };

            await _context.Orders.AddAsync(order);

            // 5. Update Stock
            foreach (var item in cart.CartItems)
                item.Product!.StockQuantity -= item.Quantity;

            // 6. Clear Cart
            _context.CartItems.RemoveRange(cart.CartItems);
            _context.Carts.Remove(cart);

            // 7. Save All
            await _context.SaveChangesAsync();

            return (order, "Order placed successfully");
        }
    }
}