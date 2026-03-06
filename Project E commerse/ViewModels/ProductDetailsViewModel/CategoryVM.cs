namespace Project_E_commerse.ViewModels.ProductDetailsViewModel
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ParentCategoryName { get; set; }
    }
}
