using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_E_commerse.Models;
using Project_E_commerse.Services.Cart;
using Project_E_commerse.Services.Order;
using Project_E_commerse.ViewModels;
using Project_E_commerse.ViewModels.CartViewModel;
using Project_E_commerse.ViewModels.ProductListVM.ProductListVM;
using System.Security.Claims;

namespace Project_E_commerse.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ILogger<CartController> logger, ICartService cartService , IOrderService orderService  , IMapper mapper)
        {
            _logger = logger;
            _cartService = cartService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");
            var (cart, msg) = await _cartService.GetCartByUserAsync(userId);
            ViewBag.Message = msg;
            if (cart == null)
                return View(new List<Cart>());

            var cartVm = _mapper.Map<CartVM>(cart);
            return View(cartVm);
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
        [HttpPost]
        public async Task<IActionResult> Update(int cartItemId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var (item, msg) = await _cartService.UpdateQuantityAsync(userId, cartItemId, quantity);

            if (item == null)
                TempData["ErrorMessage"] = msg;
            else
                TempData["SuccessMessage"] = msg;

            return RedirectToAction("Index", "Cart");
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
