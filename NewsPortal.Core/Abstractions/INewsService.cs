using NewsPortal.Core.Models;

namespace NewsPortal.Application.Services
{
    public interface INewsService
    {
        Task<Guid> CreateNews(News news);
        Task<Guid> DeleteNews(Guid id);
        Task<List<News>> GetAllNews();
        Task<Guid> UpdateNews(Guid id, string title, string categoryName, DateTime createdAt, string shortPhrase, string description);
    }
}