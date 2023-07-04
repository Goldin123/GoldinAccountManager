using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Model
{
    public static class ApplicationMessages
    {
        public static readonly string AccountHasZeroBalanceError = $"Account has zero balance.";
        public static readonly string AccountRedisKey = $"Accounts_{DateTime.Now:yyyyMMdd_hh}";
        public static readonly string AmountShouldBeGreaterThanZero = $"Amount should greater than zero.";
        public static readonly string BankingDetailsEntry = $"Please enter banking details.";
        public static readonly string CardDetailsEntry = $"Please enter credit card details.";
        public static readonly string DebitDetailsEntry = $"Please enter debit details.";
        public static readonly string AccountDetailsEntry = $"Please enter account details.";
        public static readonly string AccountNotExistError = $"Account does not exist.";
        public static readonly string AccountAlreadyExistError = "Account with id {0} already exist.";
        public static readonly string DateFromGreaterThanDateToError = $"Date from must be greater or equal than date to.";
        public static readonly string InsufficientFundsAvailable = $"Account has insufficient funds available.";
        public static readonly string NoAccountsFound = $"No accounts found.";
        public static readonly string AddedAccount = "Successfully added account with id {0} to the database.";
        public static readonly string FoundAccount = "Successfully retrieved account with id {0} on the database.";
        public static readonly string UpdateAccountDetails = "Successfully updated account with id {0} on the database.";
        public static readonly string UpdateAccountBalance = "About to updated account balance for account with id {0} on the database from {1} to {2}.";
        public static readonly string AddedAccountsToRedis = $"Added accounts to redis cache.";
        public static readonly string AddedAccountToRedis = $"Added account to redis cache.";
        public static readonly string LoadingFromDatabase = $"Loading data from the database.";
        public static readonly string LoadingFromCache = $"Loading data from redis cache.";
        public static readonly string AddedTransaction = "Added {2} transaction with id {0} for account {1}.";
        public static readonly string PerformingCreditAccountByBank = "About to perform Credit Account by Bank transfer.";
        public static readonly string PerformingCreditAccountByCard = "About to perform Credit Account by Card payment.";
        public static readonly string PerformingDebit = "About to perform debit to account with id {0}.";
        public static readonly string StatementDetails = "Pulling statement for account with id {0} on the date range {1} and {2} resulting in balance {3}.";
    }
}
