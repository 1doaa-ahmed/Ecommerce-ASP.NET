namespace Project_E_commerse.ViewModels.CheckoutViewModel
{
    public class AddressVM
    {
        public int AddressId { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public string FullAddress => $"{Street}, {City}, {Country} {Zip}";
    }
}
