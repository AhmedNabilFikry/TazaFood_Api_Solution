using AutoMapper;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Identity;
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

            CreateMap<Address, AddressDto>();
        }
    }
}
