using Project_E_commerse.Models;
using Project_E_commerse.ViewModels.ProductListVM.ProductListVM;

namespace Project_E_commerse.ViewModels.ProductDetailsViewModel
{
    public class ProductDetailsVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Category
        public CategoryVM CategoryVM { get; set; }
        // Related Products
        public IEnumerable<ProductItemVM> RelatedProducts { get; set; } = new List<ProductItemVM>();
    }
}
