using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Services;
using NewsPortal.Contracts;
using NewsPortal.Core.Models;

namespace NewsPortal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet]
        public async Task<ActionResult<List<NewsResponse>>> GetNews()
        {
            var news = await _newsService.GetAllNews();
            var response = news.Select(n => new NewsResponse(n.Id, n.Title, n.Category, n.CategoryId, n.CreatedAt, n.ShortPhrase, n.ShortPhrase));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateNews([FromBody] NewsRequest request)
        {
            var (news, error) = News.Create(
                Guid.NewGuid(),
                request.Title,
                request.Category,
                request.CategoryId,
                request.CreatedAt,
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
        public async Task<ActionResult<Guid>> UpdateNews(Guid id, [FromBody] NewsRequest request)
        {
            var newsId = await _newsService.UpdateNews(id, request.Title, request.Category.Name, request.CreatedAt, request.ShortPhrase, request.Description);
            return Ok(newsId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteNews(Guid id)
        {
            return Ok(await _newsService.DeleteNews(id));
        }
    }
}
