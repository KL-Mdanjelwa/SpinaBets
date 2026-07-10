using SpinaBets.Models;
using Microsoft.EntityFrameworkCore;
using SpinaBets.Services.Interfaces;
using SpinaBets.DbContext;

namespace SpinaBets.Services
{
    public class BetService : IBetService
    {
        private readonly ApplicationDbContext _context;

        public BetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task PlaceBetAsync(Bet bet)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == bet.AccountId);

            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.GameId == bet.GameId);

            if (account == null)
                throw new Exception("Account not found.");

            if (account.IsClosed)
                throw new Exception("Account is closed.");

            if (game == null)
                throw new Exception("Game not found.");

            if (game.Status != "Open")
                throw new Exception("Betting is closed for this game.");

            if (bet.Stake <= 0)
                throw new Exception("Stake must be greater than zero.");

            if (account.Balance < bet.Stake)
                throw new Exception("Insufficient balance.");

            //  SAFE ODDS
            bet.Odds = bet.Selection switch
            {
                "Home" => game.HomeOdds,
                "Away" => game.AwayOdds,
                "Draw" => game.DrawOdds ?? 0,
                _ => throw new Exception("Invalid selection.")
            };

            // Deduct stake
            account.Balance -= bet.Stake;

            bet.Status = BetStatus.Pending;
            bet.PlacedDate = DateTime.UtcNow;

            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Bet>> GetByAccountAsync(int accountId)
        {
            return await _context.Bets
                .Include(b => b.Game)
                .Where(b => b.AccountId == accountId)
                .OrderByDescending(b => b.PlacedDate)
                .ToListAsync();
        }

        public async Task<Bet?> GetByIdAsync(int betId)
        {
            return await _context.Bets
                .Include(b => b.Game)
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.BetId == betId);
        }

        public async Task SettleBetAsync(int betId, string result)
        {
            var bet = await _context.Bets
                .Include(b => b.Account)
                .Include(b => b.Game)
                .FirstOrDefaultAsync(b => b.BetId == betId);

            if (bet == null)
                throw new Exception("Bet not found.");

            if (bet.Status != BetStatus.Pending)
                throw new Exception("Bet already settled.");

            bet.Game.Result = result;
            bet.Game.Status = "Completed";

            if (bet.Selection == result)
            {
                bet.Status = BetStatus.Won;
                bet.Account.Balance += bet.PotentialWin;
            }
            else
            {
                bet.Status = BetStatus.Lost;
            }

            bet.SettledDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

    }
}
