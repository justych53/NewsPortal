using NewsPortal.Core.Models;
namespace NewsPortal.Contracts
{
    public record NewsRequest(

        string Title,
        Guid CategoryId,
        string ShortPhrase,
        string Description
        );

}