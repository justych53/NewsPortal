using NewsPortal.Core.Models;

namespace NewsPortal.DataAccess.Repositories
{
    public interface IImageRepository
    {
        Task<Guid> Create(Image image);
        Task<Guid> Delete(Guid id);
        Task<List<Image>> Get();
        Task<Guid> Update(Guid id, string url);
    }
}