using System.ComponentModel.DataAnnotations;

namespace UFS_BANK_FINAL.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [UIHint("Name")]
        public string UserName { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }

    public class CreateModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Address")]
        [MaxLength(50)]
        public string Address { get; set; }
        [Required]
        [Display(Name = "City")]
        [MaxLength(40)]
        public string City { get; set; }

        [Required]
        [Display(Name = "PostCode")]
        [MaxLength(4)]
        public string PostCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }

        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Display(Name = "Initial Balance")]
        public double InitialBalance { get; set; }
    }
}
