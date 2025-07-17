using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                .AsNoTracking()
                .ToListAsync();
            var news = newsEntities
                .Select(x => News.Create(x.Id, x.Title, CategoryEntity.FromEntity(x.Category), x.CategoryId, x.CreatedAt, x.ShortPhrase, x.Description).news)
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
                Description = news.Description,
                Category = CategoryEntity.MapCategoryToEntity(news.Category)
            };
            await _context.News.AddAsync(newsEntity);
            await _context.SaveChangesAsync();

            return newsEntity.Id;
        }
        public async Task<Guid> Update(Guid id, string title, string categoryName, DateTime createdAt, string shortPhrase, string description)
        {
            var categoryId = await _context.Categories
                .Where(c => c.Name == categoryName)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
            if (categoryId == Guid.Empty)
            {
                throw new Exception("Category not found");
            }
            await _context.News
                .Where(n => n.Id == id)
                .ExecuteUpdateAsync(n => n
                .SetProperty(n => n.Title, n => title)
                .SetProperty(n => n.CategoryId, n => categoryId)
                .SetProperty(n => n.CreatedAt, n => createdAt)
                .SetProperty(n => n.ShortPhrase, n => shortPhrase)
                .SetProperty(n => n.Description, n => description));
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
