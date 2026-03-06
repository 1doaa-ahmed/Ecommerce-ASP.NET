using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_E_commerse.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public int ShippingAddressId { get; set; }

        [ForeignKey("ShippingAddressId")]
        public Address? ShippingAddress { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }


    }
}
