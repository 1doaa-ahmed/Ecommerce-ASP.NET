using Microsoft.EntityFrameworkCore;
using Project_E_commerse.Data;
using Project_E_commerse.Models;
using Project_E_commerse.Repositories;

namespace Project_E_commerse.Services.Category
{
    public class CategoryService : Repository<Project_E_commerse.Models.Category>, ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project_E_commerse.Models.Category>> GetAllWithParentAsync()
        {
            return await _dbSet
                .Include(c => c.ParentCategory)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project_E_commerse.Models.Category>> GetTopLevelCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.ParentCategoryId == null)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project_E_commerse.Models.Category>> GetSubCategoriesAsync(int parentId)
        {
            return await _dbSet
                .Where(c => c.ParentCategoryId == parentId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Project_E_commerse.Models.Category?> GetDetailsById(int categoryId)
        {
            return await _dbSet
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Project_E_commerse.Models.Category category)
        {
            var existingCategory = await _context.Categories
                .FindAsync(category.CategoryId);

            if (existingCategory == null)
                return;

            existingCategory.Name = category.Name;
            existingCategory.ParentCategoryId = category.ParentCategoryId;

            await _context.SaveChangesAsync();
        }
    }
}