using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UFS_BANK_FINAL.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public static class TransactionTypeExtensions
    {
        public static string GetDisplayName(this TransactionType transactionType)
        {
            return transactionType switch
            {
                TransactionType.Deposit => "Deposit",
                TransactionType.Withdrawal => "Withdrawal",
                TransactionType.Transfer => "Transfer",
                _ => "Unknown"
            };
        }
    }


    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate unique TransactionID
        public int TransactionID { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; } 

        [Required]
        [Display(Name = "Destination Account")]
        public int DestAccount { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date Modified")]
        public DateTime ModifyDate { get; set; }

        public int AccountNumber { get; set; }
        public Account Account { get; set; }
    }
}
