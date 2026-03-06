using Project_E_commerse.Repositories.Interfaces;

namespace Project_E_commerse.Services.Category
{
    public interface ICategoryService : IRepository<Project_E_commerse.Models.Category>
    {
        Task<IEnumerable<Project_E_commerse.Models.Category>> GetTopLevelCategoriesAsync();
        Task<IEnumerable<Project_E_commerse.Models.Category>> GetSubCategoriesAsync(int parentId);
        Task<IEnumerable<Project_E_commerse.Models.Category>> GetAllWithParentAsync();
        Task<Project_E_commerse.Models.Category?> GetDetailsById(int categoryId);
        Task DeleteAsync(int id);
        Task UpdateAsync(Models.Category category);
        Task<(Models.Category? category, string msg)> AddCategoryAsync(Models.Category category);
    }
}