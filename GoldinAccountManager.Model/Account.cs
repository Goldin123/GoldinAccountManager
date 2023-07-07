using System.ComponentModel.DataAnnotations;

namespace GoldinAccountManager.Model
{
    /// <summary>
    /// Account class with all account related sub-classes or main classes.
    /// </summary>
    public class Account
    {
        [Key]
        public int AccountID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Telephone { get; set; }
        [Required]
        [MinLength(13),MaxLength(13)]
        public string IdentityNumber { get; set; }
        public decimal Balance { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }


    public class AccountRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Telephone { get; set; }
        [Required]
        [MinLength(13), MaxLength(13)]
        public string IdentityNumber { get; set; }
    }
    public class AccountDetails
    {
        public Account? Account { get; set; }
        public List<Transaction>? Transactions { get; set; }

    }

}