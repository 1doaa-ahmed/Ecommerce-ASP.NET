using Microsoft.AspNetCore.Identity;
using Project_E_commerse.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    public ICollection<Order>? Orders { get; set; } = new List<Order>();
    public ICollection<Address>? Addresses { get; set; } = new List<Address>();
    public Cart? Cart { get; set; } 
}