using NewsPortal.Core.Models;

namespace NewsPortal.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<Guid> Create(Category category);
        Task<Guid> Delete(Guid id);
        Task<List<Category>> Get();
        Task<Guid> Update(Guid id, string name);
    }
}