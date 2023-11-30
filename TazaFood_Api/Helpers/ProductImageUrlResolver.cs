using AutoMapper;
using TazaFood.Core.Models;
using TazaFood_Api.Dtos;

namespace TazaFood_Api.Helpers
{
    public class ProductImageUrlResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductImageUrlResolver(IConfiguration configuration)
        {
           _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.ImageUrl}";
            }
            return string.Empty;
        }
    }
}
