using AutoMapper;
using Project_E_commerse.Models;
using Project_E_commerse.ViewModels.ProductListViewModel;

namespace Project_E_commerse.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateVM, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ReverseMap()
                .ForMember(dest => dest.Categories, opt => opt.Ignore()); // تجاهل الـ Dropdown
        }
    }
}
