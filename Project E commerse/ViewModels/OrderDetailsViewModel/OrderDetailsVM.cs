using Project_E_commerse.Models;
using Project_E_commerse.ViewModels.CheckoutViewModel;

namespace Project_E_commerse.ViewModels.OrderDetailsViewModel
{
    public class OrderDetailsVM
    {
        // ==================== Order Header ====================
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        // ==================== Shipping Address ====================
        public AddressVM ShippingAddress { get; set; } = new();

        // ==================== Order Items ====================
        public IEnumerable<OrderItemVM> OrderItems { get; set; } = new List<OrderItemVM>();

        // ==================== Totals ====================
        public decimal SubTotal => OrderItems.Sum(i => i.LineTotal);
        public decimal Tax => SubTotal * 0.14m;
        public decimal Total => SubTotal + Tax;
        public int TotalItems => OrderItems.Sum(i => i.Quantity);
    }
}
