using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Services;
using TazaFood.Core.Specification;
using TazaFood_Api.Dtos;
using TazaFood_Api.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Product = TazaFood.Core.Models.Product;

namespace TazaFood_Api.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        //private readonly IGenericRepository<Product> _productRepo;

        public ProductController(IMapper mapper , IUnitOfWork unitOfWork ,IWebHostEnvironment webHostEnvironment, IFileService fileService  ) 
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        [CachedAttribute(100)]
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts() 
        {
            var Spec = new ProductWithCategorySpec();
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecASync(Spec);
            //var Products = await _productRepo.GetAllASync();
            // Return the Result 
            //OkObjectResult result = new OkObjectResult(Products); 
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products));
        }

        [CachedAttribute(100)]
        [HttpGet("{ID}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProductByID(int ID)
        {
            var Spec = new ProductWithCategorySpec(ID);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            //var Product = await _productRepo.GetByIDAsync(ID);
            return Ok(_mapper.Map<Product,ProductToReturnDTO>(Product));
        }

        //GetProductsOrderBy
        [HttpGet("SortingProducts")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetProducts(string Sort)
        {
            var Spec = new ProductWithCategorySpec(Sort);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecASync(Spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(Products));
        }

        //Filter Products Based on Given Criteria           
        [HttpGet("FilterProducts")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> FilterProducts(int? Price, int? CategoryID, int? Rate)
        {
            var Spec = new ProductWithCategorySpec(Price, CategoryID , Rate);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecASync(Spec);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(Products));
        }

        [CachedAttribute(100)]
        //GetPagedProducts "Add Standard Response for GetAll EndPoint"
        [HttpGet("ProductsPagination")]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProductByPagination([FromQuery]ProductSpecParams SpecParams)
        {
            var Spec = new ProductWithCategorySpec(SpecParams);
            var Products = await _unitOfWork.Repository<Product>().GetAllWithSpecASync(Spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products);
            var countspec = new ProductWithFiltrationForPaginationSpecification(SpecParams) ;
            var count = await _unitOfWork.Repository<Product>().GetCountwithSpecAsync(countspec);
            return Ok(new Pagination<ProductToReturnDTO>(SpecParams.PageIndex,SpecParams.PageSize, data , count));
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductToReturnDTO>> CreateProduct([FromForm] ProductCreateDto productCreateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Specifying the File Directory where the image will be saved 
                    string imagePath = await UploadImage.SaveImage(productCreateDto.formFile, this._webHostEnvironment.WebRootPath, productCreateDto.Name);

                    Product product = new Product()
                    {
                        Name = productCreateDto.Name,
                        Description = productCreateDto.Description,
                        Rate = productCreateDto.Rate,
                        Price = productCreateDto.Price,
                        CategoryID = productCreateDto.CategoryId,
                        ImageUrl = ImageUrlResolver.ResolveUrl(imagePath)
                    };

                    await _unitOfWork.Repository<Product>().Add(product);
                    await _unitOfWork.Complete();
                    var productDto = _mapper.Map<ProductToReturnDTO>(product);
                    return Ok(productDto);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in CreateProduct: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ProductToReturnDTO>> UpdateProduct(int productId,[FromForm] ProductCreateDto productCreateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieving the existence product from Database
                    var existingProduct = await _unitOfWork.Repository<Product>().GetByIDAsync(productId);

                    // Checking The existingProduct
                    if (existingProduct == null)
                    {
                        return NotFound(" There's No Product with this Id ");
                    }

                    // Updating The existingProduct
                    existingProduct.Name = productCreateDto.Name;
                    existingProduct.Description = productCreateDto.Description;
                    existingProduct.Rate = productCreateDto.Rate;
                    existingProduct.Price = productCreateDto.Price;
                    existingProduct.CategoryID = productCreateDto.CategoryId;

                    // Check if a new image is provided and update the image URL
                    if (productCreateDto.formFile != null)
                    {
                        string imagePath = await UploadImage.SaveImage(productCreateDto.formFile, this._webHostEnvironment.WebRootPath, productCreateDto.Name);
                        existingProduct.ImageUrl = ImageUrlResolver.ResolveUrl(imagePath);
                    }

                    // Save Changes To Database 
                    await _unitOfWork.Complete();

                    // Map the updated product to ProductToReturnDTO using AutoMapper
                    var updatedProductDto = _mapper.Map<ProductToReturnDTO>(existingProduct);

                    return Ok(updatedProductDto);

                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in UpdateProduct: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                // Retrieving the existence product from Database 
                var existingProduct = await _unitOfWork.Repository<Product>().GetByIDAsync(productId);

                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {productId} not found");
                }

                // Delete the associated image file
                await _fileService.DeleteIamge(existingProduct.ImageUrl);

                // Remove the product from the repository
                 _unitOfWork.Repository<Product>().Delete(existingProduct);

                // Save Changes To The Database
                await _unitOfWork.Complete();

                return Ok("Product and associated image deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in DeleteProduct: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
