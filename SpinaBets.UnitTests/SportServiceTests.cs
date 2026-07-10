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
    public class SportServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task CreateSport_ShouldAddSport()
        {
            // Arrange
            var context = GetDbContext();
            var service = new SportService(context);

            var sport = new Sport
            {
                Name = "Soccer"
            };

            // Act
            await service.CreateAsync(sport);

            // Assert
            Assert.AreEqual(1, context.Sports.Count());
            Assert.AreEqual("Soccer", context.Sports.First().Name);
        }

        [TestMethod]
        public async Task UpdateSport_ShouldChangeName()
        {
            // Arrange
            var context = GetDbContext();

            var sport = new Sport
            {
                SportId = 1,
                Name = "Football"
            };

            context.Sports.Add(sport);
            await context.SaveChangesAsync();

            var service = new SportService(context);

            // Act
            sport.Name = "Soccer";

            await service.UpdateAsync(sport);

            // Assert
            var updated = await context.Sports.FindAsync(1);

            Assert.AreEqual("Soccer", updated.Name);
        }
    }
}
