using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GoldinAccountManager.Database.Abstract
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IAccountRepository _account;

        public TransactionRepository(IAccountRepository account)
        {
            _account = account;
        }
        public async Task<GoldinAccountManager.Model.Transaction> CreditAccountByBankAsync(BankEFTRequest bankEFTRequest)
        {

            throw new NotImplementedException();
        }

        public async Task<GoldinAccountManager.Model.Transaction> CreditAccountByCardAsync(CrebitByCardRequest crebitByCardRequest)
        {
            try
            {
                var account = await _account.GetAccountByIdAsync(crebitByCardRequest.AccountId);
                if (account == null)
                    throw new Exception("Account does not exists");

                //Do credit card validations

                //if validation successfully passed store credit card details then add transaction
                var newTransaction = new GoldinAccountManager.Model.Transaction
                {
                    AccountID = crebitByCardRequest.AccountId,
                    Amount = crebitByCardRequest.Amount,
                    TransactioDate = DateTime.Now,
                    TransactioTypeId = (int)TransactionType.Credit,
                };

                return await AddTransaxtion(newTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<GoldinAccountManager.Model.Transaction> DebitAccountAsync(DebitRequest debitRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GoldinAccountManager.Model.Transaction>> GetAccountStatementAsync(CrebitByCardRequest crebitByCardRequest)
        {
            throw new NotImplementedException();
        }

        private async Task<GoldinAccountManager.Model.Transaction> AddTransaxtion(GoldinAccountManager.Model.Transaction transaction)
        {

            using (var db = new GoldinAccountMangerContext())
            {
                using var sqlTransaction = await db.Database.BeginTransactionAsync();

                try
                {
                    var newDBTransaction = new GoldinAccountManager.Model.Transaction
                    {
                        AccountID = transaction.AccountID,
                        Amount = transaction.Amount,
                        TransactioDate = transaction.TransactioDate,
                        TransactioTypeId = transaction.TransactioTypeId,

                    };
                    db.Transactions.Add(newDBTransaction);
                    await db.SaveChangesAsync();
                    await sqlTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await sqlTransaction.RollbackAsync();
                    return new Model.Transaction();
                }
            }
            return new Model.Transaction();

        }
    }

    //add credit card details
    //add banking information
    //get credit card
    //get bank details
}

