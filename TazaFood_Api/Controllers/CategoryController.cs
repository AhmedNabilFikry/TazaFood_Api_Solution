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
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IGenericRepository<Category> genericRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork )
        {
            _genericRepository = genericRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
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

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = new Category() 
                    { 
                        Name = newCategory.Name,
                        Description = newCategory.Description
                    };

                    await _unitOfWork.Repository<Category>().Add(category);
                    await _unitOfWork.Complete();
                    return Ok("newCategory Added Successfully" + category);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in CreateCategory: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory(int Id, Category updatedCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _unitOfWork.Repository<Category>().GetByIDAsync(Id);
                    if (category == null)
                    {
                        return NotFound("Category not found.");
                    }

                    category.Name = updatedCategory.Name;
                    category.Description = updatedCategory.Description;
                    await _unitOfWork.Complete();

                    return Ok("Updated Successfully");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in UpdateCategory: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int ID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existedCategory = await _unitOfWork.Repository<Category>().GetByIDAsync(ID);

                    if (existedCategory == null)
                    {
                        return NotFound("Category with ID {categoryId} not found.");
                    }

                    _unitOfWork.Repository<Category>().Delete(existedCategory);
                    await _unitOfWork.Complete();

                    return Ok("Category deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in DeleteCategory: {ex.Message}");
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
