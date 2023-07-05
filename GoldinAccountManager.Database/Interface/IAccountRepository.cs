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
        /// <summary>
        /// Interface that allows you to add a list accounts to the database.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<List<Account>> AddAccountsAsync(List<AccountRequest> account);
        /// <summary>
        /// Interface that allows you to add an account to the database.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Account> AddAccountAsync(AccountRequest account);
        /// <summary>
        /// Interface that returns a list of all accounts.
        /// </summary>
        /// <returns></returns>
        Task<List<Account>> GetAllAccountsAsync();
        /// <summary>
        /// Interface that returns account details based on an identity number.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Account> GetAccountByIdentityNumberAsync(string id); 
        /// <summary>
        /// Interface that returns account details based an accoundID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Account> GetAccountByIdAsync(int id);
        /// <summary>
        /// Interface that updated account details.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<Account> UpdateAccountAsync(AccountRequest account);
        /// <summary>
        /// Interface that updates an account balance.
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="amount"></param>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        Task UpdateAccountBalanceAsync(int accountId, decimal amount, TransactionType transactionType);
    }
}
