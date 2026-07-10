using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services;

namespace SpinaBets.IntegrationTests
{
    [TestClass]
    public class SportIntegrationTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task CreateSport_Should_Save_To_Database()
        {
            var context = GetDbContext();

            var service = new SportService(context);

            var sport = new Sport
            {
                Name = "Soccer"
            };

            await service.CreateAsync(sport);

            Assert.AreEqual(1, context.Sports.Count());
            Assert.AreEqual("Soccer", context.Sports.First().Name);
        }

        [TestMethod]
        public async Task EditSport_Should_Update_Name()
        {
            var context = GetDbContext();

            var sport = new Sport
            {
                Name = "Football"
            };

            context.Sports.Add(sport);
            await context.SaveChangesAsync();

            var service = new SportService(context);

            sport.Name = "Soccer";

            await service.UpdateAsync(sport);

            var updated = await context.Sports.FindAsync(sport.SportId);

            Assert.AreEqual("Soccer", updated!.Name);
        }

        [TestMethod]
        public async Task DeleteSport_Should_Remove_Record()
        {
            var context = GetDbContext();

            var sport = new Sport
            {
                Name = "Rugby"
            };

            context.Sports.Add(sport);
            await context.SaveChangesAsync();

            var service = new SportService(context);

            await service.DeleteAsync(sport.SportId);

            Assert.AreEqual(0, context.Sports.Count());
        }

        [TestMethod]
        public async Task GetAll_Should_Return_All_Sports()
        {
            var context = GetDbContext();

            context.Sports.AddRange(
                new Sport { Name = "Soccer" },
                new Sport { Name = "Cricket" },
                new Sport { Name = "Rugby" });

            await context.SaveChangesAsync();

            var service = new SportService(context);

            var sports = await service.GetAllAsync();

            Assert.AreEqual(3, sports.Count);
        }

        [TestMethod]
        public async Task GetById_Should_Return_Correct_Sport()
        {
            var context = GetDbContext();

            var sport = new Sport
            {
                Name = "Tennis"
            };

            context.Sports.Add(sport);
            await context.SaveChangesAsync();

            var service = new SportService(context);

            var result = await service.GetByIdAsync(sport.SportId);

            Assert.IsNotNull(result);
            Assert.AreEqual("Tennis", result!.Name);
        }
    }
}