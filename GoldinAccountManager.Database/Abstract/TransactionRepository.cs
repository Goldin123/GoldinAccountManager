using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var account = await _account.GetAccountByIdAsync(bankEFTRequest.AccountId);

                if (account == null)
                    throw new Exception("Account does not exists");

                //Verify if banking details 

                //Store banking details

                //Add Transaction

                var newTransaction = new GoldinAccountManager.Model.Transaction
                {
                    AccountID = bankEFTRequest.AccountId,
                    Amount = bankEFTRequest.Amount,
                    TransactioDate = DateTime.Now,
                    TransactioTypeId = (int)TransactionType.Credit,
                };

                var trans = await AddTransaxtion(newTransaction);

                await _account.UpdateAccountBalanceAsync(trans.AccountID, trans.Amount, TransactionType.Credit);

                return trans;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
            
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

                var trans =  await AddTransaxtion(newTransaction);

                await _account.UpdateAccountBalanceAsync(trans.AccountID, trans.Amount, TransactionType.Credit);
                
                return trans;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<GoldinAccountManager.Model.Transaction> DebitAccountAsync(DebitRequest debitRequest)
        {
            try 
            {
                var existingAccount = await _account.GetAccountByIdAsync(debitRequest.AccountId);
                
                if(existingAccount == null)
                    throw new Exception("Account does not exist.");
               
                if(existingAccount.Balance<= 0)
                    throw new Exception("Account has a zero balance.");

                if((existingAccount.Balance - debitRequest.Amount)<0)
                    throw new Exception("Insufficient funds available.");

                var newTransaction = new GoldinAccountManager.Model.Transaction
                {
                    AccountID = debitRequest.AccountId,
                    Amount = debitRequest.Amount,
                    TransactioDate = DateTime.Now,
                    TransactioTypeId = (int)TransactionType.Debit,
                };

                var trans = await AddTransaxtion(newTransaction);

                await _account.UpdateAccountBalanceAsync(trans.AccountID, trans.Amount, TransactionType.Debit);

                return trans;

            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
        }

        public async Task<GoldinAccountManager.Model.AccountStatement> GetAccountStatementAsync(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                if (dateFrom > dateTo)
                {
                    var existingAccount = await _account.GetAccountByIdAsync(accountId);
                    if (existingAccount != null)
                    {
                        var accountStatement = new AccountStatement(accountId, 0, dateFrom, dateTo);

                        using (var db = new GoldinAccountMangerContext())
                        {
                            var transactions = await (from a in db.Transactions
                                                      where a.AccountID ==accountId && a.TransactioDate>=dateFrom && a.TransactioDate <= dateTo
                                                      select a).ToListAsync();
                            
                            accountStatement.Transactions = transactions;
                        
                            accountStatement.AccountTotal = transactions.Sum(a => a.TransactioTypeId == 1 ? a.Amount : (-1 * a.Amount));
                        }
                        return accountStatement;
                    }
                    else
                        throw new Exception("Account does not exist.");
                }
                else
                    throw new Exception("Date from must be greater than date to.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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

