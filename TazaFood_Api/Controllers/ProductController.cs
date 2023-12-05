using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Specification;
using TazaFood_Api.Dtos;
using TazaFood_Api.Helpers;

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

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts() 
        {
            var Spec = new ProductWithCategorySpec();
            var Products = await _productRepo.GetAllWithSpecASync(Spec);
            //var Products = await _productRepo.GetAllASync();
            // Return the Result 
            //OkObjectResult result = new OkObjectResult(Products); 
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products));
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductByID(int ID)
        {
            var Spec = new ProductWithCategorySpec(ID);
            var Product = await _productRepo.GetByIDWithSpecAsync(Spec);
            //var Product = await _productRepo.GetByIDAsync(ID);
            return Ok(_mapper.Map<Product,ProductToReturnDTO>(Product));
        }
        //GetProductsOrderBy
        [HttpGet("SortingProducts")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetProducts(string Sort)
        {
            var Spec = new ProductWithCategorySpec(Sort);
            var Products = await _productRepo.GetAllWithSpecASync(Spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(Products));
        }

        //Filter Products Based on Given Criteria           
        [HttpGet("FilterProducts")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> FilterProducts(int? Price, int? CategoryID, int? Rate)
        {
            var Spec = new ProductWithCategorySpec(Price, CategoryID , Rate);
            var Products = await _productRepo.GetAllWithSpecASync(Spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(Products));
        }

        //GetPagedProducts "Add Standard Response for GetAll EndPoint"
        [HttpGet("ProductsPagination")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProductByPagination([FromQuery]ProductSpecParams SpecParams)
        {
            var Spec = new ProductWithCategorySpec(SpecParams);
            var Products = await _productRepo.GetAllWithSpecASync(Spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products);
            var countspec = new ProductWithFiltrationForPaginationSpecification(SpecParams) ;
            var count = await _productRepo.GetCountwithSpecAsync(countspec);
            return Ok(new Pagination<ProductToReturnDTO>(SpecParams.PageIndex,SpecParams.PageSize, data , count));
        }
    }
}
