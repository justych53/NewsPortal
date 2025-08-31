using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Services;
using NewsPortal.Contracts;
using NewsPortal.Core.Models;

namespace NewsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService; 

        public NewsController(INewsService newsService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService; 
        }

        [HttpGet]
        public async Task<ActionResult<List<NewsResponse>>> GetNews()
        {
            var news = await _newsService.GetAllNews();
            var response = news.Select(n => new NewsResponse(
                n.Id,
                n.Title,
                n.CategoryId,
                n.ShortPhrase,
                n.Description,
                n.CreatedAt
            ));

            return Ok(response);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NewsResponse>> GetNewsById(Guid id)
        {
            var news = await _newsService.GetNewsById(id);

            if (news == null)
            {
                return NotFound();
            }

            var response = new NewsResponse(
                news.Id,
                news.Title,
                news.CategoryId,
                news.ShortPhrase,
                news.Description,
                news.CreatedAt
            );

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateNews([FromBody] NewsRequest request)
        {
            // Проверяем существование категории
            var categoryExists = await _categoryService.CategoryExists(request.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category not found");
            }

            var (news, error) = News.Create(
                Guid.NewGuid(),
                request.Title,
                request.CategoryId,
                DateTime.UtcNow,
                request.ShortPhrase,
                request.Description
            );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var newsId = await _newsService.CreateNews(news);
            return Ok(newsId);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateNews(Guid id, [FromBody] NewsRequest request)
        {
            // Проверяем существование категории
            var categoryExists = await _categoryService.CategoryExists(request.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category not found");
            }

            var newsId = await _newsService.UpdateNews(
                id,
                request.Title,
                request.CategoryId, 
                request.ShortPhrase,
                request.Description
            );
            return Ok(newsId);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> DeleteNews(Guid id)
        {
            return Ok(await _newsService.DeleteNews(id));
        }
    }
}