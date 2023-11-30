using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Specification;

namespace TazaFood_Api.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductController( IGenericRepository<Product> productRepo) 
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() 
        {
            var Spec = new ProductWithCategorySpec();
            var Products = await _productRepo.GetAllWithSpecASync(Spec);
            //var Products = await _productRepo.GetAllASync();
            // Return the Result 
            //OkObjectResult result = new OkObjectResult(Products); 
            return Ok(Products);
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult<Product>> GetProductByID(int ID)
        {
            var Spec = new ProductWithCategorySpec(ID);
            var Product = await _productRepo.GetByIDWithSpecAsync(Spec);
            //var Product = await _productRepo.GetByIDAsync(ID);
            return Ok(Product);
        }
    }
}
