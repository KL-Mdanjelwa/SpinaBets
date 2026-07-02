namespace SpinaBets.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalAccounts { get; set; }

        public int TotalGames { get; set; }

        public int OpenGames { get; set; }

        public int CompletedGames { get; set; }

        public int TotalBets { get; set; }

        public int PendingBets { get; set; }

        public decimal TotalBalances { get; set; }
    }
}
