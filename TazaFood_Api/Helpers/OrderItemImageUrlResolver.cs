using AutoMapper;
using AutoMapper.Execution;
using TazaFood.Core.Models;
using TazaFood.Core.Models.Order_Aggregate;
using TazaFood_Api.Dtos;

namespace TazaFood_Api.Helpers
{
    public class OrderItemImageUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.ImageUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.ImageUrl}";
            }
            return string.Empty;
        }
    }
}
