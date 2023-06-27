using System.ComponentModel.DataAnnotations;

namespace GoldinAccountManager.Model
{
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Telephone { get; set; }
        [Required]
        [MinLength(13), MaxLength(13)]
        public string IdentityNumber { get; set; }
    }
}