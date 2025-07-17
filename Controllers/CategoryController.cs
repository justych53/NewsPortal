using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Services;
using NewsPortal.Contracts;
using NewsPortal.Core.Models;

namespace NewsPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var response = categories.Select(n => new CategoryResponse(n.Id, n.Name));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCategories([FromBody] CategoryRequest request)
        {
            var (category, error) = Category.Create(
                Guid.NewGuid(),
                request.Name
                );
            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }
            var categoryId = await _categoryService.CreateCategory(category);
            return Ok(categoryId);
        }
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateCategory(Guid id, [FromBody] CategoryRequest request)
        {
            var categoryId = await _categoryService.UpdateCategory(id, request.Name);
            return Ok(categoryId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteCategory(Guid id)
        {
            return Ok(await _categoryService.DeleteCategory(id));
        }
    }
}
