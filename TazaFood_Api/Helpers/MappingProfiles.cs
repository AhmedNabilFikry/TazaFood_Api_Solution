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

            CreateMap<Order, OrderTOReturnDto>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.Cost, O => O.MapFrom(S => S.DeliveryMethod.Cost))
                .ForMember(D => D.DeliveryTime, O => O.MapFrom(S => S.DeliveryMethod.DeliveryTime));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.ImageUrl, O => O.MapFrom(S => S.Product.ImageUrl))
                .ForMember(D => D.ImageUrl, O => O.MapFrom<OrderItemImageUrlResolver>());
        }
    }
}
