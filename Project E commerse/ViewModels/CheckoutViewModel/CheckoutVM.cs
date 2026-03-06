using Project_E_commerse.Models;
using Project_E_commerse.ViewModels.CartViewModel;

namespace Project_E_commerse.ViewModels.CheckoutViewModel
{
    public class CheckoutVM
    {
        public CartVM Cart { get; set; } = new();
        public IEnumerable<AddressVM> Addresses { get; set; } = new List<AddressVM>();
        public int SelectedAddressId { get; set; }
    }
}
