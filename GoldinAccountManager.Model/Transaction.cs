﻿using System.ComponentModel.DataAnnotations;

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
        public decimal Amount { get; set; }
    }

    public class AccountStatement
    {
        public int AccountId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal AccountTotal { get; set; }
        public List<Transaction> Transactions { get; set; }
        public AccountStatement(int accountId, decimal accountTotal, DateTime dateFrom, DateTime dateTo)
        {
            AccountId = accountId;
            AccountTotal = accountTotal;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Transactions = new List<Transaction>();
        }
    }

    public class AccountStatementRequest 
    {
        public int AccountId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        
    }

    public enum CardType
    {
        Debit = 1,
        Credit = 2,
    }

    public enum TransactionType
    {
        Debit = 1,
        Credit = 2
    }
    public enum Bank
    {
        StandardBank = 1,
        FnB = 2,
        Absa = 3,
        Capitec = 4,
        Nedbank = 5
    }
}
