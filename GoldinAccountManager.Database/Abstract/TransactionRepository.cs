using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Helper;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoldinAccountManager.Database.Abstract
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IAccountRepository _account;
        private readonly ILogger<TransactionRepository> _logger;
        private readonly IDistributedCache _cache;
        private readonly string _transactionsRedisrecordKey = ApplicationMessages.TransactionRedisKey;

        public TransactionRepository(IAccountRepository account, ILogger<TransactionRepository> logger, IDistributedCache cache)
        {
            _account = account;
            _logger = logger;
            _cache = cache;
        }
        public async Task<GoldinAccountManager.Model.Transaction> CreditAccountByBankAsync(BankEFTRequest bankEFTRequest)
        {
            try
            {
                var account = await _account.GetAccountByIdAsync(bankEFTRequest.AccountId);

                if (account == null)
                {
                    _logger.LogError(ApplicationMessages.AccountNotExistError);
                    throw new Exception(ApplicationMessages.AccountNotExistError);
                }

                //Verify if banking details 

                //Store banking details

                //Add Transaction
                _logger.LogInformation(ApplicationMessages.PerformingCreditAccountByBank);
                var newTransaction = new GoldinAccountManager.Model.Transaction
                {
                    AccountID = bankEFTRequest.AccountId,
                    Amount = bankEFTRequest.Amount,
                    TransactioDate = DateTime.Now,
                    TransactioTypeId = (int)TransactionType.Credit,
                };

                var trans = await AddTransaction(newTransaction);
                await _account.UpdateAccountBalanceAsync(trans.AccountID, trans.Amount, TransactionType.Credit);

                return trans;

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new NotImplementedException();
            }

        }

        public async Task<GoldinAccountManager.Model.Transaction> CreditAccountByCardAsync(CrebitByCardRequest crebitByCardRequest)
        {
            try
            {
                var account = await _account.GetAccountByIdAsync(crebitByCardRequest.AccountId);
                if (account == null)
                {
                    _logger.LogError(ApplicationMessages.AccountNotExistError);
                    throw new Exception(ApplicationMessages.AccountNotExistError);
                }

                //Do credit card validations

                //if validation successfully passed store credit card details then add transaction
                _logger.LogInformation(ApplicationMessages.PerformingCreditAccountByCard);

                var newTransaction = new GoldinAccountManager.Model.Transaction
                {
                    AccountID = crebitByCardRequest.AccountId,
                    Amount = crebitByCardRequest.Amount,
                    TransactioDate = DateTime.Now,
                    TransactioTypeId = (int)TransactionType.Credit,
                };

                var trans = await AddTransaction(newTransaction);
                await _account.UpdateAccountBalanceAsync(trans.AccountID, trans.Amount, TransactionType.Credit);

                return trans;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<GoldinAccountManager.Model.Transaction> DebitAccountAsync(DebitRequest debitRequest)
        {
            try
            {
                var existingAccount = await _account.GetAccountByIdAsync(debitRequest.AccountId);

                if (existingAccount == null)
                {
                    _logger.LogError(ApplicationMessages.AccountNotExistError);
                    throw new Exception(ApplicationMessages.AccountNotExistError);
                }

                if (existingAccount.Balance <= 0)
                {
                    _logger.LogError(ApplicationMessages.AccountHasZeroBalanceError);
                    throw new Exception(ApplicationMessages.AccountHasZeroBalanceError);
                }

                if ((existingAccount.Balance - debitRequest.Amount) < 0)
                {
                    _logger.LogError(ApplicationMessages.InsufficientFundsAvailable);
                    throw new Exception(ApplicationMessages.InsufficientFundsAvailable);
                }

                _logger.LogInformation(ApplicationMessages.PerformingDebit, existingAccount.AccountID);

                var newTransaction = new GoldinAccountManager.Model.Transaction
                {
                    AccountID = debitRequest.AccountId,
                    Amount = debitRequest.Amount,
                    TransactioDate = DateTime.Now,
                    TransactioTypeId = (int)TransactionType.Debit,
                };

                var trans = await AddTransaction(newTransaction);
                await _account.UpdateAccountBalanceAsync(trans.AccountID, trans.Amount, TransactionType.Debit);

                return trans;

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        private async Task<List<Model.Transaction>> GetCachedTransactions()
        {
            try 
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    if (db.Transactions.Count() > 0)
                    {
                        var cachedTranasctions = await _cache.GetRecordAsync<dynamic>(_transactionsRedisrecordKey);
                        var jsonTransactions = Convert.ToString(cachedTranasctions) as string;
                        if (!string.IsNullOrEmpty(jsonTransactions))
                        {
                            if (jsonTransactions.StartsWith("["))
                            {
                                var trans = JsonConvert.DeserializeObject<List<Model.Transaction>>(jsonTransactions);
                                if (trans != null)
                                    if (trans.Count() == db.Transactions.Count())
                                    {
                                        _logger.LogInformation(ApplicationMessages.LoadingFromCache);
                                        return trans;

                                    }
                            }
                        }
                    }
                    return new List<Model.Transaction>();
                }
            }
            catch (Exception ex) 
            {
                _logger.LogCritical(ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        private async Task<List<Model.Transaction>> GetAllDabaseTransactions()
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    if (db.Transactions?.Count() > 0)
                    {
                        var transactions = await db.Transactions.ToListAsync();
                        if (transactions.Count() > 1)
                        {
                            _logger.LogInformation(ApplicationMessages.AddedTransactionsToRedis);
                            await _cache.SetRecordAsync(_transactionsRedisrecordKey, transactions);
                        }
                        _logger.LogInformation(ApplicationMessages.LoadingFromDatabase);
                        return transactions;
                    }
                    else
                    {
                        _logger.LogInformation(ApplicationMessages.NoTransactionsFound);
                        return new List<Model.Transaction>();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<GoldinAccountManager.Model.AccountStatement> GetAccountStatementAsync(AccountStatementRequest accountStatementRequest)
        {
            try
            {
                if (accountStatementRequest.DateFrom <= accountStatementRequest.DateTo)
                {
                    var existingAccount = await _account.GetAccountByIdAsync(accountStatementRequest.AccountId);
                    if (existingAccount != null)
                    {
                        var accountStatement = new AccountStatement(accountStatementRequest.AccountId, 0, accountStatementRequest.DateFrom, accountStatementRequest.DateTo);

                        List<Model.Transaction> transactions = new List<Transaction>();

                        var cachedTrans = await GetCachedTransactions();
                        if (cachedTrans.Count() > 1)
                            transactions = cachedTrans;
                        else
                            transactions = await GetAllDabaseTransactions();

                        transactions = (from a in transactions
                                        where a.AccountID == accountStatementRequest.AccountId
                                        where a.TransactioDate >= accountStatementRequest.DateFrom
                                        where a.TransactioDate < accountStatementRequest.DateTo
                                        select a).ToList();

                        accountStatement.Transactions = transactions;

                        accountStatement.AccountTotal = accountStatement.Transactions.Sum(a => a.TransactioTypeId == 2 ? a.Amount : (-1 * a.Amount));
                        return accountStatement;
                    }
                    else
                    {
                        _logger.LogError(ApplicationMessages.AccountNotExistError);
                        throw new Exception(ApplicationMessages.AccountNotExistError);
                    }
                }
                else
                {
                    _logger.LogError(ApplicationMessages.DateFromGreaterThanDateToError);
                    throw new Exception(ApplicationMessages.DateFromGreaterThanDateToError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        private async Task<GoldinAccountManager.Model.Transaction> AddTransaction(GoldinAccountManager.Model.Transaction transaction)
        {

            using (var db = new GoldinAccountMangerContext())
            {
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
                    _logger.LogInformation(string.Format(ApplicationMessages.AddedTransaction, newDBTransaction.TransactionID, newDBTransaction.AccountID, transaction.TransactioTypeId == 1 ? "debit" : "credit"));
                    return newDBTransaction;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.ToString());
                    return new Model.Transaction();
                }
            }
        }
    }
}

