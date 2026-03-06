using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_E_commerse.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [ForeignKey(nameof(ParentCategory))]
        public int? ParentCategoryId { get; set; }

        // Navigation
        public Category? ParentCategory { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
