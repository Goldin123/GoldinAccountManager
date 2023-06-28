using GoldinAccountManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Database.Interface
{
    public interface IAccountRepository
    {
        Task<Account> AddAccountAsync(AccountRequest account);
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(string id);  
        Task<Account> GetAccountByIdAsync(int id);
        Task<Account> UpdateAccountAsync(AccountRequest account);
        Task UpdateAccountBalanceAsync(int accountId, decimal amount, TransactionType transactionType);
    }
}
