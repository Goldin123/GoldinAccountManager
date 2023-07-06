using GoldinAccountManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Database.Interface
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Interface that credits an account by card.
        /// </summary>
        /// <param name="crebitByCardRequest"></param>
        /// <returns></returns>
        Task<Transaction> CreditAccountByCardAsync(CrebitByCardRequest crebitByCardRequest);
        /// <summary>
        /// Interface that credits an accont by bank.
        /// </summary>
        /// <param name="bankEFTRequest"></param>
        /// <returns></returns>
        Task<Transaction> CreditAccountByBankAsync(BankEFTRequest bankEFTRequest);
        /// <summary>
        /// Interface that debits an account by an amount.
        /// </summary>
        /// <param name="debitRequest"></param>
        /// <returns></returns>
        Task<Transaction> DebitAccountAsync(DebitRequest  debitRequest);
        /// <summary>
        /// Interface that returns an account statement.
        /// </summary>
        /// <param name="accountStatementRequest"></param>
        /// <returns></returns>
        Task<AccountStatement> GetAccountStatementAsync(AccountStatementRequest accountStatementRequest);
        /// <summary>
        /// Interface that returns all transactions in the database.
        /// </summary>
        /// <returns></returns>
        Task<List<Transaction>> GetAllTransactionsAsync();
        /// <summary>
        /// Interface that returns all transactions in the database associated with an accountID.
        /// </summary>
        /// <returns></returns>
        Task<List<Model.Transaction>> GetAccountTransactionsAsync(int accounId);
    }
}
