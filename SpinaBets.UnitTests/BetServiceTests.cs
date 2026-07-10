
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services;



namespace SpinaBets.UnitTests
{
    [TestClass]
    public class BetServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task PlaceBetAsync_ShouldDeductStake()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                AccountId = 1,
                Balance = 500,
                IsClosed = false
            };

            var game = new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2.00m,
                AwayOdds = 3.00m,
                DrawOdds = 4.00m
            };

            context.Accounts.Add(account);
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var service = new BetService(context);

            var bet = new Bet
            {
                AccountId = 1,
                GameId = 1,
                Stake = 100,
                Selection = "Home"
            };

            // Act
            await service.PlaceBetAsync(bet); 

            // Assert
            Assert.AreEqual(400m, account.Balance);
            Assert.AreEqual(1, context.Bets.Count());
            Assert.AreEqual(2.00m, bet.Odds);
            Assert.AreEqual(BetStatus.Pending, bet.Status);
        }

        [TestMethod]
        public async Task PlaceBetAsync_ShouldThrow_WhenInsufficientBalance()
        {
            // Arrange
            var context = GetDbContext();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 50,
                IsClosed = false
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2.00m,
                AwayOdds = 3.00m
            });

            await context.SaveChangesAsync();

            var service = new BetService(context);

            var bet = new Bet
            {
                AccountId = 1,
                GameId = 1,
                Stake = 100,
                Selection = "Home"
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => service.PlaceBetAsync(bet));
        }

        [TestMethod]
        public async Task PlaceBetAsync_ShouldThrow_WhenAccountClosed()
        {
            // Arrange
            var context = GetDbContext();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 500,
                IsClosed = true
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2.00m,
                AwayOdds = 3.00m
            });

            await context.SaveChangesAsync();

            var service = new BetService(context);

            var bet = new Bet
            {
                AccountId = 1,
                GameId = 1,
                Stake = 100,
                Selection = "Home"
            };

            // Act & Assert
            await  Assert.ThrowsAsync<Exception>(
                () => service.PlaceBetAsync(bet));
        }
        [TestMethod]
        public async Task SettleBetAsync_ShouldCreditWinningBet()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                AccountId = 1,
                Balance = 100
            };

            var game = new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2.00m,
                AwayOdds = 3.00m
            };

            var bet = new Bet
            {
                BetId = 1,
                AccountId = 1,
                Account = account,
                GameId = 1,
                Game = game,
                Stake = 100,
                Odds = 2.00m,
                Selection = "Home",
                Status = BetStatus.Pending
            };

            context.Accounts.Add(account);
            context.Games.Add(game);
            context.Bets.Add(bet);

            await context.SaveChangesAsync();

            var service = new BetService(context);

            // Act
            await service.SettleBetAsync(1, "Home");

            // Assert
            Assert.AreEqual(BetStatus.Won, bet.Status);
            Assert.AreEqual(300m, account.Balance);
            Assert.AreEqual("Completed", game.Status);
            Assert.AreEqual("Home", game.Result);
        }

        [TestMethod]
        public async Task SettleBetAsync_ShouldLoseWithoutCrediting()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                AccountId = 1,
                Balance = 100
            };

            var game = new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2.00m,
                AwayOdds = 3.00m
            };

            var bet = new Bet
            {
                BetId = 1,
                AccountId = 1,
                Account = account,
                GameId = 1,
                Game = game,
                Stake = 100,
                Odds = 2.00m,
                Selection = "Away",
                Status = BetStatus.Pending
            };

            context.Accounts.Add(account);
            context.Games.Add(game);
            context.Bets.Add(bet);

            await context.SaveChangesAsync();

            var service = new BetService(context);

            // Act
            await service.SettleBetAsync(1, "Home");

            // Assert
            Assert.AreEqual(BetStatus.Lost, bet.Status);
            Assert.AreEqual(100m, account.Balance);
        }
    }
}