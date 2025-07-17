using NewsPortal.Core.Models;

namespace NewsPortal.Application.Services
{
    public interface IImageService
    {
        Task<Guid> CreateImage(Image image);
        Task<Guid> DeleteImage(Guid id);
        Task<List<Image>> GetAllImages();
        Task<Guid> UpdateImage(Guid id, string name);
    }
}