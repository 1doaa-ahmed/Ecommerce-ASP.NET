using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_E_commerse.Models;
using Project_E_commerse.Services.Category;
using Project_E_commerse.Services.Product;
using Project_E_commerse.ViewModels.ProductListVM;
using Project_E_commerse.ViewModels.ProductListVM.ProductListVM;
using System.Globalization;

namespace Project_E_commerse.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;


        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService ,  IProductService productService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? categoryId, decimal? minPrice, decimal? maxPrice, string? sortBy, bool inStockOnly = false)
        {
            var categories = await _categoryService.GetTopLevelCategoriesAsync();

            IEnumerable<Product> products = categoryId.HasValue
                ? await _productService.GetProductsByCategoryAsync(categoryId.Value)
                : await _productService.GetAllAsync();

            // Filter by Price
            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            // Filter by Stock
            if (inStockOnly)
                products = products.Where(p => p.StockQuantity > 0);

            // Sort
            products = sortBy switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "name" => products.OrderBy(p => p.Name),
                _ => products
            };
            var model = new ProductListVM
            {
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                InStockOnly = inStockOnly,
                Categories = categories.Select(c => new CategoryItemVM
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name
                }).ToList(),
                Products = products.Select(p => new ProductItemVM
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    SKU = p.SKU,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    IsActive = p.IsActive,
                    CategoryName = p.Category?.Name
                }).ToList(),
                CurrentPage = 1,
                TotalPages = 1,
                TotalItems = products.Count()
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
            var (addedCategory, msg) = await _categoryService.AddCategoryAsync(category);
            if (addedCategory is null)
            {
                ModelState.AddModelError("Name", msg);
                return View(category);
            }
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