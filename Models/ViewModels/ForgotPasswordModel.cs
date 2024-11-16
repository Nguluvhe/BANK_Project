using System.ComponentModel.DataAnnotations;

namespace UFS_BANK_FINAL.Models.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
