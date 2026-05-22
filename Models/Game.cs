namespace SpinaBets.Models
{
    public class Game
    {
        public int GameId { get; set; }

        public int SportId { get; set; }
        
        public Sport Sport { get; set; }

        public string HomeTeam { get; set; } = "";

        public string AwayTeam { get; set; } = "";

        public DateTime StartTime { get; set; }

        public decimal HomeOdds { get; set; }

        public decimal AwayOdds { get; set; }

        public decimal? DrawOdds { get; set; }

        public string Status { get; set; } = "";

        public string? Result { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}
