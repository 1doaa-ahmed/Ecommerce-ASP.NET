using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_E_commerse.Models;
using Project_E_commerse.Services.Category;
using Project_E_commerse.Services.Product;
using Project_E_commerse.ViewModels.ProductListViewModel;

namespace Project_E_commerse.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public ProductController(ILogger<ProductController> logger, IProductService productService , ICategoryService categoryService , IMapper mapper  )
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductCreateVM productVm)
        {
            //if (!ModelState.IsValid)
            //{
            //    productVm.Categories = await _categoryService.GetAllAsync();
            //    return View(productVm);
            //}
            Product product = _mapper.Map<Product>(productVm); 
            var (addedProduct, msg) = await _productService.AddProductAsync(product);
            if (addedProduct is null)
            {
                ModelState.AddModelError("Name", msg ?? "Something went wrong, please try again.");
                productVm.Categories = await _categoryService.GetAllAsync(); 
                return View(productVm);
            }
            TempData["SuccessMessage"] = $"Product '{addedProduct.Name}' created successfully!";
            return RedirectToAction(nameof(Create));
        }
        //[HttpPost]
        //public async Task<IActionResult> Create(ProductCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.Categories = await _categoryService.GetAllAsync(); // re-fill on error
        //        return View(model);
        //    }

        //    var product = new Product
        //    {
        //        Name = model.Name,
        //        SKU = model.SKU,
        //        Price = model.Price,
        //        StockQuantity = model.StockQuantity,
        //        CategoryId = model.CategoryId
        //    };

        //    await _productService.AddAsync(product);
        //    return RedirectToAction("Index");
        //}


        public async Task<IActionResult> Create()
        {
            var model = new ProductCreateVM
            {
                Categories = await _categoryService.GetAllAsync()
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            await _productService.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (success, msg) = await _productService.DeleteAsync(c => c.ProductId == id);
            if (!success)
            {
                TempData["Error"] = msg;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = msg;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
