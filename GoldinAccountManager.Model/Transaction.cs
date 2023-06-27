using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Model
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public int MyProperty { get; set; }
        public int TransactioTypeId { get; set; }
        public DateTime TransactioDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class CrebitByCardRequest 
    {
        public int CardTypeId { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string Expiry { get; set; }
        public int CVV { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }

    public class BankEFTRequest 
    {
        public int BankId { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
    }

    public class DebitRequest 
    {
        public int AccountId { get; set; }
        public decimal  Amount { get; set; }
    }

    public enum CardType 
    {
        Debit,
        Credit,
    }
    
    public enum TransactionType 
    {
        Debit,
        Credit 
    }
    public enum Bank 
    {
        StandardBank,
        FnB,
        Absa,
        Capitec,
        Nedbank
    }
}
