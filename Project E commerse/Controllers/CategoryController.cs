using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_E_commerse.Models;
using Project_E_commerse.Services.Category;
using Project_E_commerse.ViewModels.ProductListVM;
using Project_E_commerse.ViewModels.ProductListVM.ProductListVM;

namespace Project_E_commerse.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var categories = await _categoryService.GetTopLevelCategoriesAsync();

            var model = new ProductListVM
            {
                CategoryId = categoryId,

                Categories = categories.Select(c => new CategoryItemVM
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList(),

                Products = new List<ProductItemVM>(),

                CurrentPage = 1,
                TotalPages = 1,
                TotalItems = 0
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            Category category = await _categoryService.GetByIdAsync(id);
            return View(category);
        }

        // =========================
        // ADMIN CRUD
        // =========================

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryService.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _categoryService.UpdateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}