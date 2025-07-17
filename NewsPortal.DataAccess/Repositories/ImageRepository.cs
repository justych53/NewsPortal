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
    public class ImageRepository : IImageRepository
    {
        private readonly NewsPortalDbContext _context;
        public ImageRepository(NewsPortalDbContext context)
        {
            _context = context;
        }
        public async Task<List<Image>> Get()
        {
            var imageEntities = await _context.Images
                .AsNoTracking()
                .ToListAsync();
            var images = imageEntities
                .Select(x => Image.Create(x.Id, x.Url))
                .ToList();
            return images;
        }
        public async Task<Guid> Create(Image image)
        {
            var imageEntity = new ImageEntity
            {
                Id = image.Id,
                Url = image.Url,
            };
            await _context.Images.AddAsync(imageEntity);
            await _context.SaveChangesAsync();

            return imageEntity.Id;
        }
        public async Task<Guid> Update(Guid id, string url)
        {
            await _context.Images
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Id, x => id)
                .SetProperty(x => x.Url, x => url));
            return id;
        }
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Images
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
