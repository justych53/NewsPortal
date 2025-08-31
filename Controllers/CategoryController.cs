using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NewsPortal.Application.Services;
using NewsPortal.Contracts;
using NewsPortal.Core.Models;

namespace NewsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            Console.WriteLine("=== GET CATEGORIES CALLED ===");
            Console.WriteLine($"Query string: {Request.QueryString}");

            var authHeader = Request.Headers["Authorization"].ToString();
            Console.WriteLine($"Auth header: {authHeader}");

            var tokenFromQuery = Request.Query["token"].ToString();
            Console.WriteLine($"Token from query: {tokenFromQuery}");

            if (!string.IsNullOrEmpty(tokenFromQuery))
            {
                Console.WriteLine("Token received from query, manually processing...");
            }

            var categories = await _categoryService.GetAllCategories();
            var response = categories.Select(n => new CategoryResponse(n.Id, n.Name));

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateCategory(Guid id, [FromBody] CategoryRequest request)
        {
            var categoryId = await _categoryService.UpdateCategory(id, request.Name);
            return Ok(categoryId);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> DeleteCategory(Guid id)
        {
            return Ok(await _categoryService.DeleteCategory(id));
        }
    }
}