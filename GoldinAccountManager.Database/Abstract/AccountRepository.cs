using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Database.Abstract
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<int> AddAccountAsync(Account account)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    db.Accounts.Add(account);
                    await db.SaveChangesAsync();
                    return account.AccountID;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> GetAccountByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
