using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace SpinaBets.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Customer()
        {
            var user = await _userManager.GetUserAsync(User);

            var accounts = await _context.Accounts
                .Where(a => a.UserId == user!.Id)
                .ToListAsync();

            var accountIds = accounts.Select(a => a.AccountId).ToList();

            var vm = new CustomerDashboardViewModel
            {
                TotalBalance = accounts.Sum(a => a.Balance),

                OpenBets = await _context.Bets
                    .CountAsync(b =>
                        accountIds.Contains(b.AccountId) &&
                        b.Status == BetStatus.Pending),

                WonBets = await _context.Bets
                    .CountAsync(b =>
                        accountIds.Contains(b.AccountId) &&
                        b.Status == BetStatus.Won),

                LostBets = await _context.Bets
                    .CountAsync(b =>
                        accountIds.Contains(b.AccountId) &&
                        b.Status == BetStatus.Lost),

                TotalTransactions = await _context.Transactions
                    .CountAsync(t =>
                        accountIds.Contains(t.AccountId)),

                RecentBets = await _context.Bets
                    .Include(b => b.Game)
                    .Where(b => accountIds.Contains(b.AccountId))
                    .OrderByDescending(b => b.PlacedDate)
                    .Take(5)
                    .ToListAsync(),

                RecentTransactions = await _context.Transactions
                    .Where(t => accountIds.Contains(t.AccountId))
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var vm = new AdminDashboardViewModel
            {
                TotalUsers = await _context.AdminDashboardReport.CountAsync(),

                TotalAccounts = await _context.Accounts.CountAsync(),

                TotalGames = await _context.Games.CountAsync(),

                OpenGames = await _context.Games
                    .CountAsync(g => g.Status == "Open"),

                CompletedGames = await _context.Games
                    .CountAsync(g => g.Status == "Completed"),

                TotalBets = await _context.Bets.CountAsync(),

                PendingBets = await _context.Bets
                    .CountAsync(b => b.Status == BetStatus.Pending),

                TotalBalances = await _context.Accounts
                    .SumAsync(a => a.Balance)
            };

            return View(vm);
        }
    }
}
