using NewsPortal.Core.Models;
namespace NewsPortal.Contracts
{
    public record NewsResponse(
    
        Guid Id,
        string Title,
        Guid CategoryId,
        string ShortPhrase,
        string Description,
        DateTime CreatedAt
        );

}
