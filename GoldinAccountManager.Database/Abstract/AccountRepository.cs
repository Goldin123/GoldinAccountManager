using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Helper;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GoldinAccountManager.Database.Abstract
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger<AccountRepository> _logger;
        private readonly IDistributedCache _cache;
        private readonly string _accountsRedisrecordKey = ApplicationMessages.AccountRedisKey;
        public AccountRepository(ILogger<AccountRepository> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public async Task<Account> AddAccountAsync(AccountRequest account)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var existingAccount = await db.Accounts.Where(a => a.IdentityNumber == account.IdentityNumber).SingleOrDefaultAsync();
                    if (existingAccount == null)
                    {
                        var newAccount = new Account
                        {
                            IdentityNumber = account.IdentityNumber,
                            Active = true,
                            Balance = 0,
                            DateCreated = DateTime.Now,
                            DateUpdated = DateTime.Now,
                            Email = account.Email,
                            FirstName = account.FirstName,
                            LastName = account.LastName,
                            Telephone = account.Telephone,
                        };

                        db.Accounts.Add(newAccount);
                        await db.SaveChangesAsync();
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.AddedAccount, newAccount.AccountID)));
                        return newAccount;
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.AccountAlreadyExistError, existingAccount.AccountID)));
                        throw new InvalidOperationException(ApplicationMessages.AccountAlreadyExistError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Account>> AddAccountsAsync(List<AccountRequest> accounts)
        {
            try
            {
                var newAccounts = new List<Account>();
                using (var db = new GoldinAccountMangerContext())
                {
                    foreach (var account in accounts)
                    {
                        var existingAccount = await db.Accounts.Where(a => a.IdentityNumber == account.IdentityNumber).SingleOrDefaultAsync();
                        if (existingAccount == null)
                        {
                            var newAccount = new Account
                            {
                                IdentityNumber = account.IdentityNumber,
                                Active = true,
                                Balance = 0,
                                DateCreated = DateTime.Now,
                                DateUpdated = DateTime.Now,
                                Email = account.Email,
                                FirstName = account.FirstName,
                                LastName = account.LastName,
                                Telephone = account.Telephone,
                            };

                            db.Accounts.Add(newAccount);
                            await db.SaveChangesAsync();
                            _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.AddedAccount, newAccount.AccountID)));
                            newAccounts.Add(newAccount);
                        }
                        else
                            _logger.LogError(string.Format(ApplicationMessages.AccountAlreadyExistError, existingAccount.AccountID));
                    }

                    if (newAccounts?.Count > 0)
                    {
                        if (newAccounts.Count() > 1)
                        {
                            _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AddedAccountsToRedis));
                            await _cache.SetRecordAsync(_accountsRedisrecordKey, newAccounts);
                        }
                        return newAccounts;
                    }
                    else
                        return new List<Account>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> GetAccountByIdentityNumberAsync(string id)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var account = await (from a in db.Accounts
                                         where a.IdentityNumber == id
                                         select new Account
                                         {
                                             AccountID = a.AccountID,
                                             FirstName = a.FirstName,
                                             LastName = a.LastName,
                                             Active = a.Active,
                                             Balance = a.Balance,
                                             DateCreated = a.DateCreated,
                                             DateUpdated = a.DateUpdated,
                                             Email = a.Email,
                                             IdentityNumber = a.IdentityNumber,
                                             Telephone = a.Telephone
                                         }).SingleOrDefaultAsync();

                    if (account?.AccountID > 0)
                    {
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.FoundAccount, account.AccountID)));
                        return account;
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountNotExistError));
                        throw new InvalidOperationException(ApplicationMessages.AccountNotExistError);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var account = await (from a in db.Accounts
                                         where a.AccountID == id
                                         select new Account
                                         {
                                             AccountID = a.AccountID,
                                             FirstName = a.FirstName,
                                             LastName = a.LastName,
                                             Active = a.Active,
                                             Balance = a.Balance,
                                             DateCreated = a.DateCreated,
                                             DateUpdated = a.DateUpdated,
                                             Email = a.Email,
                                             IdentityNumber = a.IdentityNumber,
                                             Telephone = a.Telephone

                                         }).SingleOrDefaultAsync();

                    if (account?.AccountID > 0)
                    {
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.FoundAccount, account.AccountID)));
                        return account;
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountNotExistError));
                        throw new InvalidOperationException(ApplicationMessages.AccountNotExistError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<Account>> GetCachedAccount() 
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    if (db.Accounts.Count() > 0)
                    {
                        var cacheAccounts = await _cache.GetRecordAsync<dynamic>(_accountsRedisrecordKey);
                        var jsonAccounts = Convert.ToString(cacheAccounts) as string;
                        if (!string.IsNullOrEmpty(jsonAccounts))
                        {
                            if (jsonAccounts.StartsWith("["))
                            {
                                var accounts = JsonConvert.DeserializeObject<List<Account>>(jsonAccounts);
                                if (accounts != null)
                                    if (accounts.Count() == db.Accounts.Count())
                                    {
                                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.LoadingFromCache));
                                        return accounts;

                                    }
                            }
                        }
                    }
                }
                return new List<Account>();
            }catch (Exception ex) 
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));    
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            try
            {
                List<Account>? accounts = null;

                using (var db = new GoldinAccountMangerContext())
                {
                    if (db.Accounts.Count() > 0)
                    {
                        var cachedAccounts = await GetCachedAccount();

                        if (cachedAccounts?.Count > 0)
                            return cachedAccounts;


                        accounts = await (from a in db.Accounts
                                          select new Account
                                          {
                                              AccountID = a.AccountID,
                                              FirstName = a.FirstName,
                                              LastName = a.LastName,
                                              Active = a.Active,
                                              Balance = a.Balance,
                                              DateCreated = a.DateCreated,
                                              DateUpdated = a.DateUpdated,
                                              Email = a.Email,
                                              IdentityNumber = a.IdentityNumber,
                                              Telephone = a.Telephone
                                          }).ToListAsync();

                        if (accounts?.Count > 0)
                        {

                            _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.LoadingFromDatabase));

                            if (accounts.Count() > 1)
                            {
                                _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AddedAccountsToRedis));
                                await _cache.SetRecordAsync(_accountsRedisrecordKey, accounts);
                            }
                            return accounts;
                        }
                        else
                        {
                            _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.NoAccountsFound));
                            accounts = new List<Account>();
                            return accounts;
                        }
                    }
                    else
                    {
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.NoAccountsFound));
                        accounts = new List<Account>();
                        return accounts;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }
        public async Task<Account> UpdateAccountAsync(AccountRequest account)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var existingAccount = await (from a in db.Accounts
                                                 where a.IdentityNumber == account.IdentityNumber
                                                 select a).SingleOrDefaultAsync();
                    if (existingAccount != null)
                    {
                        existingAccount.FirstName = account.FirstName;
                        existingAccount.LastName = account.LastName;
                        existingAccount.IdentityNumber = account.IdentityNumber;
                        existingAccount.Telephone = account.Telephone;
                        existingAccount.Email = account.Email;
                        existingAccount.DateUpdated = DateTime.Now;
                        await db.SaveChangesAsync();
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.UpdateAccountDetails, existingAccount.AccountID)));
                        return existingAccount;
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountNotExistError));
                        throw new InvalidOperationException(ApplicationMessages.AccountNotExistError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }
        public async Task<Account> UpdateAccountAsync(Account account)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var existingAccount = await (from a in db.Accounts
                                                 where a.IdentityNumber == account.IdentityNumber && a.AccountID == account.AccountID
                                                 select a).SingleOrDefaultAsync();
                    if (existingAccount != null)
                    {
                        existingAccount.FirstName = account.FirstName;
                        existingAccount.LastName = account.LastName;
                        existingAccount.IdentityNumber = account.IdentityNumber;
                        existingAccount.Telephone = account.Telephone;
                        existingAccount.Email = account.Email;
                        existingAccount.DateUpdated = DateTime.Now;
                        existingAccount.Balance = account.Balance;
                        await db.SaveChangesAsync();
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.UpdateAccountDetails, existingAccount.AccountID)));

                        return existingAccount;
                    }
                    else
                    {
                        _logger.LogError(string.Format("{0} - {1}", DateTime.Now, ApplicationMessages.AccountNotExistError));
                        throw new InvalidOperationException(ApplicationMessages.AccountNotExistError);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAccountBalanceAsync(int accountId, decimal amount, TransactionType transactionType)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var existingAccount = await GetAccountByIdAsync(accountId);
                    if (existingAccount != null)
                    {
                        var currentBalace = existingAccount.Balance;
                        if (transactionType == TransactionType.Debit)
                            amount = -1 * amount;

                        existingAccount.Balance = existingAccount.Balance + amount;
                        _logger.LogInformation(string.Format("{0} - {1}", DateTime.Now, string.Format(ApplicationMessages.UpdateAccountBalance, existingAccount.AccountID, currentBalace, existingAccount.Balance)));
                        await UpdateAccountAsync(existingAccount);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(string.Format("{0} - {1}", DateTime.Now, ex.Message));
                throw new Exception(ex.Message);
            }
        }
    }
}
