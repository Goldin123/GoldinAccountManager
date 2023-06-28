using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoldinAccountManager.Database.Abstract
{
    public class AccountRepository : IAccountRepository
    {
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
                        return newAccount;
                    }
                    else
                    {
                        return new Account();
                    }
                }
            }
            catch (Exception ex)
            {
                return new Account();
            }
        }

        public async Task<Account> GetAccountByIdAsync(string id)
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
                    return account;
                }

            }
            catch (Exception ex)
            {
                return new Account();
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
                    return account;
                }

            }
            catch (Exception ex)
            {
                return new Account();
            }
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var account = await (from a in db.Accounts
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
                    return account;
                }

            }
            catch (Exception ex)
            {
                return new List<Account>();
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
                        return existingAccount;
                    }
                    return new Account();
                }
            }
            catch (Exception ex)
            {
                return new Account();
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
                        await db.SaveChangesAsync();
                        return existingAccount;
                    }
                    return new Account();
                }
            }
            catch (Exception ex)
            {
                return new Account();
            }
        }

        public async Task UpdateAccountBalanceAsync(int accountId, decimal amount, TransactionType transactionType)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    using var sqlTransaction = await db.Database.BeginTransactionAsync();

                    try
                    {
                        var existingAccount = await GetAccountByIdAsync(accountId);
                        if (existingAccount != null) 
                        {
                            if(transactionType ==TransactionType.Debit)
                                amount = -1 * amount;
                            
                            existingAccount.Balance = existingAccount.Balance +  amount;

                            await UpdateAccountAsync(existingAccount);
                            
                            await sqlTransaction.CommitAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await sqlTransaction.RollbackAsync();
                    }
                }
            } catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
