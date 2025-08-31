using NewsPortal.Core.Models;

namespace NewsPortal.DataAccess.Repositories
{
    public interface INewsRepository
    {
        Task<Guid> Create(News news);
        Task<Guid> Delete(Guid id);
        Task<List<News>> Get();
        Task<Guid> Update(Guid id, string title, Guid categoryId, string shortPhrase, string description);
    }
}