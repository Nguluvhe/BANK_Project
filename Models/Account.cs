using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UFS_BANK_FINAL.Models
{
    public enum AccountType
    {
        Checking = 'C',
        Saving = 'S'
    }

    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required]
        [Display(Name = "Type")]
        public char AccountType { get; set; }


        [Required, DataType(DataType.Currency)]
        [Display(Name = "Account Balance")]
        public double Balance { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Date Modified")]
        public DateTime ModifyDate { get; set; }

        public string Advice { get; set; }

        public string UserId { get; set; }

        public string CustomerName { get; set; }
        public Customer Customer { get; set; }

        public List<Transaction> Transactions { get; set; }
        public List<Feedback> Feedbacks { get; set; }

    }
}
