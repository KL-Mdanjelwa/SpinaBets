using SpinaBets.Models;

namespace SpinaBets.DTO
{
    public class BettingReportDto
    {
        public int BetId { get; set; }

        public string AccountNumber { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string Selection { get; set; }

        public decimal Stake { get; set; }

        public decimal Odds { get; set; }

        public BetStatus Status { get; set; }

        public DateTime PlacedDate { get; set; }
    }
}
