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
        public static readonly string AmountShouldBeGreaterThanZero = $"Amount should greater than zero.";
        public static readonly string BankingDetailsEntry = $"Please enter banking details.";
        public static readonly string CardDetailsEntry = $"Please enter credit card details.";
        public static readonly string DebitDetailsEntry = $"Please enter debit details.";
        public static readonly string AccountDetailsEntry = $"Please enter account details.";
        public static readonly string AccountNotExistError = $"Account does not exist.";
        public static readonly string AccountAlreadyExistError = $"Account already exist.";
        public static readonly string DateFromGreaterThanDateToError = $"Date from must be greater or equal than date to.";
        public static readonly string InsufficientFundsAvailable = $"Account has insufficient funds available.";
        public static readonly string NoAccountsFound = $"No accounts found.";
    }
}
