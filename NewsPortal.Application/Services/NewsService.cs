using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Guid> UpdateNews(Guid id, string title, Guid categoryId, string shortPhrase, string description)
        {
            return await _newsRepository.Update(id, title, categoryId, shortPhrase, description);
        }

        public async Task<Guid> DeleteNews(Guid id)
        {
            return await _newsRepository.Delete(id);
        }

        public async Task<News?> GetNewsById(Guid id)
        {
            var allNews = await _newsRepository.Get();
            return allNews.FirstOrDefault(n => n.Id == id);
        }
    }
}