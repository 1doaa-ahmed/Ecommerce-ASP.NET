using Microsoft.AspNetCore.Mvc;
using Project_E_commerse.Models;
using Project_E_commerse.Repositories.Interfaces;
using Project_E_commerse.ViewModels.ProductListViewModel;

namespace Project_E_commerse.Services.Product
{
    public interface IProductService : IRepository<Project_E_commerse.Models.Product>
    {
        Task<(IEnumerable<Project_E_commerse.Models.Product> Items, int TotalCount)> GetPagedAsync(
            int? categoryId, string? search, string? sort, int page, int pageSize);
        Task<Project_E_commerse.Models.Product?> GetWithCategoryAsync(int id);
        Task<IEnumerable<Project_E_commerse.Models.Product>> GetByCategoryAsync(int categoryId);
        Task<bool> IsSkuUniqueAsync(string sku, int? excludeId = null);
        Task<(Project_E_commerse.Models.Product? product, string msg)> AddProductAsync(Project_E_commerse.Models.Product product);
        Task UpdateAsync(Models.Product product);
        Task<IEnumerable<Project_E_commerse.Models.Product>> GetProductsByCategoryAsync(int categoryId);

    }
}