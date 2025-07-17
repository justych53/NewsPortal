using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<List<Image>> GetAllImages()
        {
            return await _imageRepository.Get();
        }
        public async Task<Guid> CreateImage(Image image)
        {
            return await _imageRepository.Create(image);
        }
        public async Task<Guid> UpdateImage(Guid id, string name)
        {
            return await _imageRepository.Update(id, name);
        }
        public async Task<Guid> DeleteImage(Guid id)
        {
            return await _imageRepository.Delete(id);
        }
    }
}
