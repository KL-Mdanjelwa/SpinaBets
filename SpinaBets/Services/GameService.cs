using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services.Interfaces;

namespace SpinaBets.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetAll()
        {
            Console.WriteLine("SERVICE HIT");
            return await _context.Games
                .Include(g => g.Sport)
                .OrderByDescending(g => g.StartTime)
                .ToListAsync();
        }

        public async Task<Game?> GetById(int id)
        {
            return await _context.Games
                .Include(g => g.Sport)
                .FirstOrDefaultAsync(g => g.GameId == id);
        }

        public async Task Create(Game game)
        {
            Console.WriteLine("Create Clicked");
            game.Status = "Open";

            _context.Games.Add(game);

            await _context.SaveChangesAsync();
            Console.WriteLine("Create Done");
        }

        public async Task Update(Game game)
        {
            var existing = await _context.Games.FindAsync(game.GameId);

            if (existing == null)
                throw new Exception("Game not found.");

            existing.SportId = game.SportId;
            existing.HomeTeam = game.HomeTeam;
            existing.AwayTeam = game.AwayTeam;
            existing.StartTime = game.StartTime;
            existing.HomeOdds = game.HomeOdds;
            existing.AwayOdds = game.AwayOdds;
            existing.DrawOdds = game.DrawOdds;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetResult(int gameId, string result)
        {
            var game = await _context.Games
                .Include(g => g.Bets)
                    .ThenInclude(b => b.Account)
                .FirstOrDefaultAsync(g => g.GameId == gameId);

            if (game == null)
                throw new Exception("Game not found.");

            if (game.Status == "Completed")
                throw new Exception("Game already settled.");

            game.Result = result;
            game.Status = "Completed";

            foreach (var bet in game.Bets.Where(b => b.Status == BetStatus.Pending))
            {
                if (bet.Selection == result)
                {
                    bet.Status = BetStatus.Won;
                    bet.Account.Balance += bet.PotentialWin;
                }
                else
                {
                    bet.Status = BetStatus.Lost;
                }

                bet.SettledDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
