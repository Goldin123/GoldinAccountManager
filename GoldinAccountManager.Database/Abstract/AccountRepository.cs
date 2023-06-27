using GoldinAccountManager.Database.DB;
using GoldinAccountManager.Database.Interface;
using GoldinAccountManager.Model;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Account> GetAccountByNameAsync(string name)
        {
            try
            {
                using (var db = new GoldinAccountMangerContext())
                {
                    var account = await (from a in db.Accounts
                                         where a.FirstName == name
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

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
