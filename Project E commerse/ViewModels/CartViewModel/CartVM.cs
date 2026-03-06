using Project_E_commerse.Models;

namespace Project_E_commerse.ViewModels.CartViewModel
{
    public class CartVM
    {
        // ==================== Items ====================
        public IEnumerable<CartItemVM> CartItems { get; set; } = new List<CartItemVM>();

        // ==================== Totals ====================
        public decimal SubTotal => CartItems.Sum(i => i.LineTotal);
        public decimal Tax => SubTotal * 0.14m;  // 14% VAT
        public decimal Total => SubTotal + Tax;
        public int TotalItems => CartItems.Sum(i => i.Quantity);

    }
}
