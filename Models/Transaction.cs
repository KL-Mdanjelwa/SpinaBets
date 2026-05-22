using System.ComponentModel.DataAnnotations;

namespace SpinaBets.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; } 

        public TransactionType TransactionType { get; set; } 
        

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime CaptureDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? Reference { get; set; } = "";

    }
}
