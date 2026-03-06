using System.ComponentModel.DataAnnotations;

namespace Project_E_commerse.ViewModels.AccountViewModel
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        [Display(Name = "FullName")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
