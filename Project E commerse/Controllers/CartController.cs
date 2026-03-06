using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_E_commerse.ViewModels;
using Project_E_commerse.Models;
using System.Security.Claims;
using Project_E_commerse.ViewModels.ProductListVM.ProductListVM;
using Project_E_commerse.Services.Cart;
using Project_E_commerse.Services.Order;

namespace Project_E_commerse.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ILogger<CartController> logger, ICartService cartService , IOrderService orderService )
        {
            _logger = logger;
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");
            var (orders, msg) = await _orderService.GetUserOrdersAsync(userId);
            ViewBag.Message = msg;
            if (orders == null)
                return View(new List<Order>());
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var (order, msg) = await _orderService.GetOrderByIdAsync(id);
            ViewBag.Message = msg;
            if (order == null)
                return RedirectToAction("Index", "Home");
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Add(int productId, int qty = 1)
        {
            // 1. Get current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");
            var (item, msg) = await _cartService.AddToCartAsync(userId, productId, qty);
            ViewBag.Message = msg;
            if (item == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Index", "Cart");
        }
        public async Task<ProductVM> Update(Product product, int qty)
        {
            // check if product id is exist
            ProductVM productDTO = new ProductVM();
            Product existProduct = await _cartService.GetByIdAsync(product.ProductId);
            if (existProduct == null)
            {
                ViewBag.Message = "Product not found!";
                return productDTO;
            }
            if (existProduct.StockQuantity < qty)
            {
                ViewBag.Message = $"Not enough stock! Available: {existProduct.StockQuantity}";
                return productDTO;
            }
            try
            {
                productDTO.product = existProduct;
                productDTO.quantity = qty;

                await _cartService.Update(existProduct);
                ViewBag.Message = "Product updated successfully";
                return (productDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed to update product: " + ex.Message;
                return productDTO;
            }
        }

        public async Task<Product> Remove(Product product)
        {
            string errMsg = string.Empty;
            // check if product id is exist
            Product existProduct = await _cartService.GetByIdAsync(product.ProductId);
            if (existProduct == null)
            {
                ViewBag.Message = "Product not found!";
                return null;
            }
            try
            {
                await _cartService.Delete(existProduct);
                ViewBag.Message = "Product Deleteted successfully";
                return existProduct;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed to Delete product: " + ex.Message;
                return null;
            }
        }
    }
}
