using NewsPortal.Core.Models;

namespace NewsPortal.Application.Services
{
    public interface ICategoryService
    {
        Task<Guid> CreateCategory(Category category);
        Task<Guid> DeleteCategory(Guid id);
        Task<List<Category>> GetAllCategories();
        Task<Guid> UpdateCategory(Guid id, string name);
    }
}