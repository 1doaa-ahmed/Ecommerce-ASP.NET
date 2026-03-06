using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_E_commerse.Services.Admin;

namespace Project_E_commerse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Products
        public async Task<IActionResult> Products()
        {
            var (products, msg) = await _adminService.GetAllProductsAsync();
            ViewBag.Message = msg;
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var (product, msg) = await _adminService.GetProductByIdAsync(id);
            ViewBag.Message = msg;
            return View(product);
        }

        // Orders
        public async Task<IActionResult> Orders()
        {
            var (orders, msg) = await _adminService.GetAllOrdersAsync();
            ViewBag.Message = msg;
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var msg = await _adminService.UpdateOrderStatusAsync(orderId, status);
            TempData["Message"] = msg;

            return RedirectToAction("Orders");
        }
    }
}
