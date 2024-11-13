using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UFS_BANK_FINAL.Models
{
    public class Customer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required (ErrorMessage = "Only letters of the alphabet")]
        [MaxLength(50)]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Must end with @bank.co.za")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Only letters and numbers")]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please only use names.")]
        [MaxLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "Four digits only")]
        [MaxLength(4)]
        public string PostCode { get; set; }

        public string Role { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
