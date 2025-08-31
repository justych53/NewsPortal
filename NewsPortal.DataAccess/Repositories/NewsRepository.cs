using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Entities;

namespace NewsPortal.DataAccess.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsPortalDbContext _context;

        public NewsRepository(NewsPortalDbContext context)
        {
            _context = context;
        }

        public async Task<List<News>> Get()
        {
            var newsEntities = await _context.News
                .Include(n => n.Category) 
                .AsNoTracking()
                .ToListAsync();

            var news = newsEntities
                .Select(x => News.Create(
                    x.Id,
                    x.Title,
                    x.CategoryId, 
                    x.CreatedAt,
                    x.ShortPhrase,
                    x.Description).news)
                .ToList();

            return news;
        }

        public async Task<Guid> Create(News news)
        {
            var newsEntity = new NewsEntity()
            {
                Id = news.Id,
                Title = news.Title,
                CategoryId = news.CategoryId, 
                CreatedAt = news.CreatedAt,
                ShortPhrase = news.ShortPhrase,
                Description = news.Description
            };

            await _context.News.AddAsync(newsEntity);
            await _context.SaveChangesAsync();

            return newsEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string title, Guid categoryId, string shortPhrase, string description)
        {
            // Проверяем существование категории
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == categoryId);

            if (!categoryExists)
            {
                throw new Exception("Category not found");
            }

            await _context.News
                .Where(n => n.Id == id)
                .ExecuteUpdateAsync(n => n
                    .SetProperty(n => n.Title, title)
                    .SetProperty(n => n.CategoryId, categoryId)
                    .SetProperty(n => n.ShortPhrase, shortPhrase)
                    .SetProperty(n => n.Description, description)
                    .SetProperty(n => n.CreatedAt, DateTime.UtcNow) // Обновляем время изменения
                );

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.News
                .Where(n => n.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}