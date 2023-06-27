using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldinAccountManager.Model
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        [Required]
        public int AccountID { get; set; }
        public int TransactioTypeId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TransactioDate { get; set; }
    }

    public class CrebitByCardRequest 
    {
        public int CardTypeId { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string NameOnCard { get; set; }
        [Required]
        public string Expiry { get; set; }
        [Required]
        public int CVV { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int AccountId { get; set; }
    }

    public class BankEFTRequest 
    {
        public int BankId { get; set; }
        [Required]
        public string AccountHolder { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int AccountId { get; set; }
    }

    public class DebitRequest 
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
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
