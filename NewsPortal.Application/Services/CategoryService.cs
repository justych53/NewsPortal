using NewsPortal.Core.Models;
using NewsPortal.DataAccess.Repositories;
namespace NewsPortal.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _categoryRepository.Get();
        }
        public async Task<Guid> CreateCategory(Category category)
        {
            return await _categoryRepository.Create(category);
        }
        public async Task<Guid> UpdateCategory(Guid id, string name)
        {
            return await _categoryRepository.Update(id, name);
        }
        public async Task<Guid> DeleteCategory(Guid id)
        {
            return await _categoryRepository.Delete(id);
        }
    }

}
