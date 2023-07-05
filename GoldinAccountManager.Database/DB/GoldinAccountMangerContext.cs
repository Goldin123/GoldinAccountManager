using GoldinAccountManager.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Database.DB
{
    /// <summary>
    /// This the the in memory database used to store accounts and transactions.
    /// </summary>
    public class GoldinAccountMangerContext : DbContext
    {
        protected override void OnConfiguring
      (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "GoldinAccountManagerDatabase");
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
      
    }
}
