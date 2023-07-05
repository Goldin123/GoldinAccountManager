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
        Task<Transaction> CreditAccountByCardAsync(CrebitByCardRequest crebitByCardRequest);
        Task<Transaction> CreditAccountByBankAsync(BankEFTRequest bankEFTRequest);
        Task<Transaction> DebitAccountAsync(DebitRequest  debitRequest);
        Task<AccountStatement> GetAccountStatementAsync(AccountStatementRequest accountStatementRequest);
        Task<List<Transaction>> GetAllTransactions();
    }
}
