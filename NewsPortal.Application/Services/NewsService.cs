using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<List<News>> GetAllNews()
        {
            return await _newsRepository.Get();
        }
        public async Task<Guid> CreateNews(News news)
        {
            return await _newsRepository.Create(news);
        }
        public async Task<Guid> UpdateNews(Guid id, string title, string categoryName, DateTime createdAt, string shortPhrase, string description)
        {
            return await _newsRepository.Update(id, title, categoryName, createdAt, shortPhrase, description);
        }
        public async Task<Guid> DeleteNews(Guid id)
        {
            return await _newsRepository.Delete(id);
        }
    }
}
