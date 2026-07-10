using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services;

namespace SpinaBets.UnitTests
{
    [TestClass]
    public class GameServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task CreateGame_Should_Add_Game()
        {
            // Arrange
            var context = GetDbContext();

            context.Sports.Add(new Sport
            {
                SportId = 1,
                Name = "Football"
            });

            await context.SaveChangesAsync();

            var service = new GameService(context);

            var game = new Game
            {
                SportId = 1,
                HomeTeam = "Chiefs",
                AwayTeam = "Pirates",
                StartTime = DateTime.UtcNow,
                HomeOdds = 2.10m,
                AwayOdds = 3.20m,
                DrawOdds = 3.00m
            };

            // Act
            await service.Create(game);

            // Assert
            Assert.AreEqual(1, context.Games.Count());
            Assert.AreEqual("Open", game.Status);
        }

        [TestMethod]
        public async Task UpdateGame_Should_Update_Game()
        {
            var context = GetDbContext();

            context.Sports.Add(new Sport
            {
                SportId = 1,
                Name = "Football"
            });

            var game = new Game
            {
                GameId = 1,
                SportId = 1,
                HomeTeam = "Chiefs",
                AwayTeam = "Pirates",
                Status = "Open",
                HomeOdds = 2m,
                AwayOdds = 3m,
                DrawOdds = 4m
            };

            context.Games.Add(game);

            await context.SaveChangesAsync();

            var service = new GameService(context);

            game.HomeTeam = "Sundowns";

            await service.Update(game);

            var updated = await context.Games.FindAsync(1);

            Assert.AreEqual("Sundowns", updated.HomeTeam);
        }

        [TestMethod]
        public async Task DeleteGame_Should_Remove_Game()
        {
            var context = GetDbContext();

            context.Sports.Add(new Sport
            {
                SportId = 1,
                Name = "Football"
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                SportId = 1,
                HomeTeam = "Chiefs",
                AwayTeam = "Pirates",
                Status = "Open"
            });

            await context.SaveChangesAsync();

            var service = new GameService(context);

            await service.Delete(1);

            Assert.AreEqual(0, context.Games.Count());
        }

        [TestMethod]
        public async Task SetResult_Should_Complete_Game()
        {
            var context = GetDbContext();

            context.Sports.Add(new Sport
            {
                SportId = 1,
                Name = "Football"
            });

            context.Games.Add(new Game
            {
                GameId = 1,
                SportId = 1,
                HomeTeam = "Chiefs",
                AwayTeam = "Pirates",
                Status = "Open"
            });

            await context.SaveChangesAsync();

            var service = new GameService(context);

            await service.SetResult(1, "Home");

            var game = await context.Games.FindAsync(1);

            Assert.AreEqual("Completed", game.Status);
            Assert.AreEqual("Home", game.Result);
        }
    }
}
