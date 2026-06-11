using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpinaBets.DbContext;
using SpinaBets.Models;
using Microsoft.EntityFrameworkCore;
using SpinaBets.ViewModels;

namespace SpinaBets.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalBets = await _context.Bets.CountAsync();
            var totalWon = await _context.Bets.CountAsync(b => b.Status == BetStatus.Won);
            var totalLost = await _context.Bets.CountAsync(b => b.Status == BetStatus.Lost);

            var totalStake = await _context.Bets.SumAsync(b => (decimal?)b.Stake) ?? 0;

            var model = new ReportViewModel
            {
                TotalBets = totalBets,
                WonBets = totalWon,
                LostBets = totalLost,
                TotalStake = totalStake
            };

            return View(model);
        }
    }
}
