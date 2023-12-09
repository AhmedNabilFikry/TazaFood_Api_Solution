using AutoMapper;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Identity;
using TazaFood.Core.Models.Order_Aggregate;
using TazaFood_Api.Dtos;

namespace TazaFood_Api.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
                .ForMember(D => D.ImageUrl, O => O.MapFrom<ProductImageUrlResolver>());

            CreateMap<TazaFood.Core.Models.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, TazaFood.Core.Models.Order_Aggregate.Address>();    
        }
    }
}
