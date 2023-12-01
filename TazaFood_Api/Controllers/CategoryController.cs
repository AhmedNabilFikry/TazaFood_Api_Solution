using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;

namespace TazaFood_Api.Controllers
{

    public class CategoryController : BaseApiController
    {
        private readonly IGenericRepository<Category> _genericRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IGenericRepository<Category> genericRepository, ICategoryRepository categoryRepository)
        {
            _genericRepository = genericRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            var Categories = await _genericRepository.GetAllASync();
            return Ok(Categories);
        }
        [HttpGet("{ID:int}")]
        public async Task<ActionResult<Category>> GetCategoryByID(int ID)
        {
            var Category = await _genericRepository.GetByIDAsync(ID);
            return Ok(Category);
        }
        [HttpGet("{Name}")]
        public async Task<ActionResult<Category>> GetCategoryByName(string Name)
        {
            var Category = await _categoryRepository.GetByName(Name);
            if (Category == null)
            {
                return NotFound("This Category is not Found");
            }
            return Ok(Category);
        }
    }
}
