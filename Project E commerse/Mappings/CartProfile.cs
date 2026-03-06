using AutoMapper;
using Project_E_commerse.Models;
using Project_E_commerse.ViewModels.CartViewModel;
using Project_E_commerse.ViewModels.ProductListViewModel;

namespace Project_E_commerse.Mappings
{
    public class CartProfile : Profile
    {
            public CartProfile()
            {
            CreateMap<CartItem, CartItemVM>()
          .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name))
          .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Product!.Category!.Name))
          .ForMember(dest => dest.LineTotal, opt => opt.Ignore()); // computed property

            CreateMap<Cart, CartVM>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            // Reverse
            CreateMap<CartVM, Cart>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

        }
    }
}
