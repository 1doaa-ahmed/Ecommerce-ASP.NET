using Microsoft.EntityFrameworkCore;
using Project_E_commerse.Data;
using Project_E_commerse.Models;
using Project_E_commerse.Repositories;

namespace Project_E_commerse.Services.Product
{
    public class ProductService : Repository<Project_E_commerse.Models.Product>, IProductService
    {
        public ProductService(ApplicationDbContext context) : base(context) { }

        public async Task<(IEnumerable<Project_E_commerse.Models.Product> Items, int TotalCount)> GetPagedAsync(
            int? categoryId, string? search, string? sort, int page, int pageSize)
        {
            var query = _dbSet
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) || p.SKU.Contains(search));

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name_desc" => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            var total = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Project_E_commerse.Models.Product?> GetWithCategoryAsync(int id)
            => await _dbSet
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

        public async Task<IEnumerable<Project_E_commerse.Models.Product>> GetByCategoryAsync(int categoryId)
            => await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();

        public async Task<bool> IsSkuUniqueAsync(string sku, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.SKU == sku);
            if (excludeId.HasValue)
                query = query.Where(p => p.ProductId != excludeId.Value);
            return !await query.AnyAsync();
        }

        public async Task<(Project_E_commerse.Models.Product? product, string msg)> AddProductAsync(Project_E_commerse.Models.Product product)
        {
            return await AddAsync(product, c => c.ProductId == product.ProductId);
        }
        public async Task UpdateAsync(Project_E_commerse.Models.Product product)
        {
            var existingProduct = await _context.Products
                .FindAsync(product.ProductId);

            if (existingProduct == null)
                return;

            existingProduct.Name = product.Name;
            existingProduct.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Project_E_commerse.Models.Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }


    }
}