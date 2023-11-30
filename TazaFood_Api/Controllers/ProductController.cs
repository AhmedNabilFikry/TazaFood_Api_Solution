using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Specification;
using TazaFood_Api.Dtos;

namespace TazaFood_Api.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductController( IGenericRepository<Product> productRepo , IMapper mapper) 
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetProducts() 
        {
            var Spec = new ProductWithCategorySpec();
            var Products = await _productRepo.GetAllWithSpecASync(Spec);
            //var Products = await _productRepo.GetAllASync();
            // Return the Result 
            //OkObjectResult result = new OkObjectResult(Products); 
            return Ok(_mapper.Map<IEnumerable<Product>,IEnumerable<ProductToReturnDTO>>(Products));
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductByID(int ID)
        {
            var Spec = new ProductWithCategorySpec(ID);
            var Product = await _productRepo.GetByIDWithSpecAsync(Spec);
            //var Product = await _productRepo.GetByIDAsync(ID);
            return Ok(_mapper.Map<Product,ProductToReturnDTO>(Product));
        }
    }
}
