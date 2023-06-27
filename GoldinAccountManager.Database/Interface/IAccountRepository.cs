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
        Task<int> AddAccountAsync(Account account);
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);  
        Task<Account> GetAccountByNameAsync(string name);
        Task<Account> UpdateAccountAsync(Account account);
    }
}
