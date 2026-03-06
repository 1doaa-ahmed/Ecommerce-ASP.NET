namespace Project_E_commerse.ViewModels.ProductListVM.ProductListVM
{
    public class ProductItemVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
