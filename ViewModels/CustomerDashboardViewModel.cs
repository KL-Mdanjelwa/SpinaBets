using SpinaBets.Models;

namespace SpinaBets.ViewModels
{
    public class CustomerDashboardViewModel
    {
        public decimal TotalBalance { get; set; }

        public int OpenBets { get; set; }

        public int WonBets { get; set; }

        public int LostBets { get; set; }

        public int TotalTransactions { get; set; }

        public List<Bet> RecentBets { get; set; } = new();

        public List<Transaction> RecentTransactions { get; set; } = new();
    }
}
