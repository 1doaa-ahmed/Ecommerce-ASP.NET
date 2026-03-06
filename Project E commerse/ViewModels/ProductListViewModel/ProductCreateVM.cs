using Project_E_commerse.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_E_commerse.ViewModels.ProductListViewModel
{
    public class ProductCreateVM
    {
       
            [Required]
            [StringLength(150)]
            public string Name { get; set; } = string.Empty;
        public int ProductId { get; set; }

        [Required]
            [StringLength(50)]
            public string SKU { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;


        [Required]
            [Column(TypeName = "decimal(18,2)")]
            public decimal Price { get; set; }

            [Required]
            public int StockQuantity { get; set; }

            [Required]
            public int CategoryId { get; set; } 

            public IEnumerable<Category>? Categories { get; set; }
        }
    }
