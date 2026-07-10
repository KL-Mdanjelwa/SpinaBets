using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services;

namespace SpinaBets.IntegrationTests
{
    [TestClass]
    public class BetIntegrationTests
    {
        private ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task Customer_Can_Place_Bet()
        {
            var context = GetDb();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 500,
                IsClosed = false
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2,
                AwayOdds = 3
            });

            await context.SaveChangesAsync();

            var service = new BetService(context);

            await service.PlaceBetAsync(new Bet
            {
                AccountId = 1,
                GameId = 1,
                Stake = 100,
                Selection = "Home"
            });

            Assert.AreEqual(1, context.Bets.Count());
        }

        [TestMethod]
        public async Task New_Bet_Should_Be_Pending()
        {
            var context = GetDb();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 500
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2
            });

            await context.SaveChangesAsync();

            var service = new BetService(context);

            await service.PlaceBetAsync(new Bet
            {
                AccountId = 1,
                GameId = 1,
                Stake = 50,
                Selection = "Home"
            });

            var bet = context.Bets.First();

            Assert.AreEqual(BetStatus.Pending, bet.Status);
        }

        [TestMethod]
        public async Task Winning_Bet_Should_Update_Status()
        {
            var context = GetDb();

            var account = new Account
            {
                AccountId = 1,
                Balance = 100
            };

            var game = new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2
            };

            var bet = new Bet
            {
                BetId = 1,
                Account = account,
                AccountId = 1,
                Game = game,
                GameId = 1,
                Stake = 100,
                Odds = 2,
                Selection = "Home",
                Status = BetStatus.Pending
            };

            context.Accounts.Add(account);
            context.Games.Add(game);
            context.Bets.Add(bet);

            await context.SaveChangesAsync();

            var service = new BetService(context);

            await service.SettleBetAsync(1, "Home");

            Assert.AreEqual(BetStatus.Won, bet.Status);
        }

        [TestMethod]
        public async Task Losing_Bet_Should_Update_Status()
        {
            var context = GetDb();

            var account = new Account
            {
                AccountId = 1,
                Balance = 100
            };

            var game = new Game
            {
                GameId = 1,
                Status = "Open",
                HomeOdds = 2
            };

            var bet = new Bet
            {
                BetId = 1,
                Account = account,
                AccountId = 1,
                Game = game,
                GameId = 1,
                Stake = 100,
                Odds = 2,
                Selection = "Away",
                Status = BetStatus.Pending
            };

            context.Accounts.Add(account);
            context.Games.Add(game);
            context.Bets.Add(bet);

            await context.SaveChangesAsync();

            var service = new BetService(context);

            await service.SettleBetAsync(1, "Home");

            Assert.AreEqual(BetStatus.Lost, bet.Status);
        }

        [TestMethod]
        public async Task Cannot_Place_Bet_On_Completed_Game()
        {
            var context = GetDb();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 500
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                Status = "Completed",
                HomeOdds = 2
            });

            await context.SaveChangesAsync();

            var service = new BetService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.PlaceBetAsync(new Bet
                {
                    AccountId = 1,
                    GameId = 1,
                    Stake = 50,
                    Selection = "Home"
                }));
        }


    }
}
