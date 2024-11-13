using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UFS_BANK_FINAL.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int AccountNumber { get; set; } 
        public string Message { get; set; }
        public DateTime SubmitDate { get; set; }
        public Account Account { get; set; }
    }

}
