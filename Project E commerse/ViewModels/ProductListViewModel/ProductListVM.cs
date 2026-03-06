using Project_E_commerse.ViewModels.ProductListVM;

namespace Project_E_commerse.ViewModels.ProductListVM.ProductListVM
{
    public class ProductListVM
    {
        // ==================== Items ====================
        public IEnumerable<ViewModels.ProductListVM.ProductListVM.ProductItemVM> Products { get; set; } = new List<ViewModels.ProductListVM.ProductListVM.ProductItemVM>();

        // ==================== Categories (for dropdown) ====================
        public IEnumerable<CategoryItemVM> Categories { get; set; } = new List<CategoryItemVM>();

        // ==================== Paging ====================
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        // ==================== Filter ====================
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; }
        public bool InStockOnly { get; set; }


    }
}
