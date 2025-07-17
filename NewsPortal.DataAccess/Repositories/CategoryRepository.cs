using Microsoft.EntityFrameworkCore;
using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsPortalDbContext _context;
        public CategoryRepository(NewsPortalDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> Get()
        {
            var categoryEntities = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            var categories = categoryEntities
                .Select(x => Category.Create(x.Id, x.Name).category)
                .ToList();
            return categories;
        }
        public async Task<Guid> Create(Category category)
        {
            var categoryEntity = new CategoryEntity
            {
                Id = category.Id,
                Name = category.Name,
            };
            await _context.Categories.AddAsync(categoryEntity);
            await _context.SaveChangesAsync();

            return categoryEntity.Id;
        }
        public async Task<Guid> Update(Guid id, string name)
        {
            await _context.Categories
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Id, x => id)
                .SetProperty(x => x.Name, x => name));
            return id;
        }
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Categories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
