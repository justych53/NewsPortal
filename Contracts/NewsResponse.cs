using NewsPortal.Core.Models;
namespace NewsPortal.Contracts
{
    public record NewsResponse(
    
        Guid Id,
        string Title,
        Category Category,
        Guid CategoryId,
        DateTime CreatedAt,
        string ShortPhrase,
        string Description
        );

}
