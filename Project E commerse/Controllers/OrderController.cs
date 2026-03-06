using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_E_commerse.ViewModels;
using Project_E_commerse.Models;
using System.Security.Claims;
using Project_E_commerse.ViewModels.CheckoutViewModel;
using Project_E_commerse.Services.Cart;
using Project_E_commerse.Services.Order;

namespace Project_E_commerse.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartService _cartService;

        public OrderController(
    ILogger<OrderController> logger,
    IOrderService orderService,
    ICartService cartService,
    UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            // 1. Get current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");
            // 2. Get cart
            var (cart, msg) = await _orderService.GetCartByUserAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                ViewBag.Message = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }
            // 3. Get user addresses
            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId);
            // 4. Build ViewModel
            var viewModel = new CheckoutVM
            {
                //Cart = cart,
                //Addresses = user?.Addresses,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutVM viewModel)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            
            var (order, msg) = await _cartService.CheckoutAsync(userId, viewModel.SelectedAddressId);

            if (order == null)
            {
                ViewBag.Message = msg;
                return View(viewModel);
            }

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }
    }
}
