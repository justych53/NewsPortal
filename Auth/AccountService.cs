using Microsoft.AspNetCore.Identity;
using NewsPortal.Models;
using NewsPortal.DataAccess.Repositories;
namespace NewsPortal.Auth
{
    public class AccountService(AccountRepository accountRepository, JwtService jwtService)
    {
        public void Register(string userName, string firstName, string password)
        {
            var account = new Account(Guid.NewGuid(), userName, firstName, string.Empty);
            var passHash = new PasswordHasher<Account>().HashPassword(account, password);
            account.PasswordHash = passHash;
            accountRepository.Add(account);
        }

        public async Task<string> Login(string userName, string password)

        {
            var account = await accountRepository.GetByUserName(userName);
            if (account == null)
            {
                throw new Exception("Unauthorized");
            }
            var result = new PasswordHasher<Account>().VerifyHashedPassword(account, account.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
                return jwtService.GenerateToken(account);
            }
            else
            {
                throw new Exception("Unauthorized");
            }
        }
    }
}
