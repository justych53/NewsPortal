using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsPortal.DataAccess.Entities;
using NewsPortal.Models;

namespace NewsPortal.DataAccess.Repositories
{
    public class AccountRepository
    {
        private readonly NewsPortalDbContext _context;
    
        public AccountRepository(NewsPortalDbContext context)
        {
            _context = context;
        }
        public async Task<Account?> GetByUserName(string userName)
        {
            var accountEntity = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(b=>b.UserName == userName);
            
            if (accountEntity == null)
            {
                return null;
            }
            return Account.Create(accountEntity.Id, accountEntity.UserName, accountEntity.FirstName, accountEntity.PasswordHash).Account;
        }
        public async Task Add(Account account)
        {
            var accountEntity = new AccountEntity
            {
                Id = account.Id,
                UserName = account.UserName,
                FirstName = account.FirstName,
                PasswordHash = account.PasswordHash,
            };
            await _context.Accounts.AddAsync(accountEntity);
            await _context.SaveChangesAsync();

        }

        //public Account? GetByUserName(string userName)
        //{
        //    List<Account> =  
        //    return Accounts.TryGetValue(userName, out var account) ? account : null;
        //}

    }
}
