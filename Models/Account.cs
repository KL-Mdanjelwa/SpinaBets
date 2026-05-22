using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace SpinaBets.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        
        public string UserId { get; set; } = "";
        public ApplicationUser User { get; set; }

        [MaxLength(100)]
        public string AccountNumber { get; set; } = "";

        public AccountType AccountType { get; set; }

        
        public decimal Balance { get; set; }

        public bool IsClosed { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<Bet> Bets { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
