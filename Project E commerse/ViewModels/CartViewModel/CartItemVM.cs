namespace Project_E_commerse.ViewModels.CartViewModel
{
    public class CartItemVM
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
        public int StockQuantity { get; set; }
        public bool IsAvailable => StockQuantity > 0;
        public string CategoryName { get; set;}

    }
}
