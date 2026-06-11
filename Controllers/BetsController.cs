using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services.Interfaces;
using SpinaBets.ViewModels;

namespace SpinaBets.Controllers
{
    [Authorize]
    public class BetsController : Controller
    {
        private readonly IBetService _betService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public BetsController(
            IBetService betService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _betService = betService;
            this.userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == user.Id);

            if (account == null)
                return View(new List<Bet>());

            var bets = await _betService.GetByAccountAsync(account.AccountId);

            return View(bets);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Games = await _context.Games
                .Where(g => g.Status == "Open")
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Games = await _context.Games
                    .Where(g => g.Status == "Open")
                    .ToListAsync();

                return View(model);
            }

            var user = await userManager.GetUserAsync(User);

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == user.Id);

            if (account == null)
                return BadRequest("No account found.");

            var bet = new Bet
            {
                AccountId = account.AccountId,
                GameId = model.GameId,
                Selection = model.Selection,
                Stake = model.Stake
            };

            try
            {
                await _betService.PlaceBetAsync(bet);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                ViewBag.Games = await _context.Games
                    .Where(g => g.Status == "Open")
                    .ToListAsync();

                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Settle()
        {
            var bets = await _context.Bets
                .Include(b => b.Game)
                .Include(b => b.Account)
                .Where(b => b.Status == BetStatus.Pending)
                .ToListAsync();

            return View(bets);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SettleBet(int betId, string result)
        {
            try
            {
                await _betService.SettleBetAsync(betId, result);
                return RedirectToAction(nameof(Settle));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Settle));
            }
        }

    }
}
