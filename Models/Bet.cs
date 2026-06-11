using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpinaBets.Models
{
    public class Bet
    {
        public int BetId { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        [Required]
        public string Selection { get; set; } = "";

        public decimal Stake { get; set; }

        
        public decimal Odds { get; set; }

        public BetStatus Status { get; set; } = BetStatus.Pending;

        [Precision(16, 2)]
        public decimal PotentialWin => Stake * Odds;

        public DateTime PlacedDate { get; set; } = DateTime.UtcNow;

        public DateTime? SettledDate { get; set; }
    }
}
