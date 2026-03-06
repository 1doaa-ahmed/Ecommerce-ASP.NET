using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_E_commerse.Data;
using Project_E_commerse.Models;
using Project_E_commerse.Repositories;
using Project_E_commerse.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Project_E_commerse.Services.Admin
{
    // AdminService.cs
    public class AdminService : Repository<Project_E_commerse.Models.Address>, IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        //Products
        public async Task<(IEnumerable<Project_E_commerse.Models.Product>? products, string msg)> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            if (!products.Any())
                return (null, "No products found!");
            return (products, $"{products.Count} products found");
        }

        public async Task<(Project_E_commerse.Models.Product? product, string msg)> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return (null, "Product not found");
            return (product, "Product found");
        }

        public async Task<(Project_E_commerse.Models.Product? product, string msg)> CreateProductAsync(Project_E_commerse.Models.Product product)
        {
            // Check category exists
            var category = await _context.Categories
                .FindAsync(product.CategoryId);
            if (category == null)
                return (null, "Category not found!");
            var existingSKU = await _context.Products
                .AnyAsync(p => p.SKU == product.SKU);
            if (existingSKU)
                return (null, "SKU already exists!");

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return (product, "Product created successfully");
        }

        public async Task<(Project_E_commerse.Models.Product? product, string msg)> UpdateProductAsync(Project_E_commerse.Models.Product product)
        {
            // Check product exists
            var existProduct = await _context.Products
                .FindAsync(product.ProductId);

            if (existProduct == null)
                return (null, "Product not found!");

            // Check category exists
            var category = await _context.Categories
                .FindAsync(product.CategoryId);

            if (category == null)
                return (null, "Category not found!");

            // Update fields
            existProduct.Name = product.Name;
            existProduct.SKU = product.SKU;
            existProduct.Price = product.Price;
            existProduct.StockQuantity = product.StockQuantity;
            existProduct.IsActive = product.IsActive;
            existProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();

            return (existProduct, "Product updated successfully");
        }

        public async Task<string> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                return "Product not found!";

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return "Product deleted successfully";
        }

        // Orders 
        public async Task<(IEnumerable<Project_E_commerse.Models.Order>? orders, string msg)> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Include(o => o.ShippingAddress)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            if (!orders.Any())
                return (null, "No orders found!");

            return (orders, $"{orders.Count} orders found ✅");
        }

        public async Task<string> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return "Order not found!";
            if (!Enum.TryParse<OrderStatus>(status, out var orderStatus))
                return $"Invalid status! Valid statuses: {string.Join(", ", Enum.GetNames<OrderStatus>())}";
            order.Status = orderStatus;
            await _context.SaveChangesAsync();
            return $"Order status updated to {orderStatus} ";
        }


    }
}