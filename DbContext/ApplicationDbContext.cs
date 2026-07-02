using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DTO;
using SpinaBets.Models;
namespace SpinaBets.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AdminDashboardReport> AdminDashboardReport { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<BettingReportDto> BettingReports { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
            builder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            builder.Entity<Bet>()
            .Ignore(b => b.PotentialWin);

            
            builder.Entity<Bet>()
                .Property(b => b.Stake)
                .HasPrecision(18, 2);

            builder.Entity<Bet>()
                .Property(b => b.Odds)
                .HasPrecision(10, 2);

            
            builder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Game>()
               .Property(g => g.HomeOdds)
               .HasPrecision(10, 2);

            builder.Entity<Game>()
                .Property(g => g.AwayOdds)
                .HasPrecision(10, 2);

            builder.Entity<Game>()
                .Property(g => g.DrawOdds)
                .HasPrecision(10, 2);

            builder.Entity<ApplicationUser>()
            .HasIndex(u => u.IDNumber)
            .IsUnique();

            builder.Entity<Account>()
           .Property(a => a.AccountType)
           .HasConversion<string>();

            builder.Entity<Transaction>()
            .Property(t => t.TransactionType)
            .HasConversion<string>();

            builder.Entity<AdminDashboardReport>()
            .HasNoKey()
            .ToView("vw_AdminDashboard");

            base.OnModelCreating(builder);

            builder.Entity<Account>()
                .HasIndex(a => a.UserId);

            builder.Entity<Bet>()
                .HasIndex(b => b.AccountId);

            builder.Entity<Bet>()
                .HasIndex(b => b.GameId);

            builder.Entity<Game>()
                .HasIndex(g => g.SportId);

            builder.Entity<Transaction>()
                .HasIndex(t => t.AccountId);

            builder.Entity<Transaction>()
                .HasIndex(t => t.TransactionDate);

            builder.Entity<Bet>()
                .HasIndex(b => b.Status);

            builder.Entity<BettingReportDto>()
                .HasNoKey();

            builder.Entity<BettingReportDto>()
                .ToView(null);

        }
    }
}
