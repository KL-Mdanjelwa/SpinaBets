using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services;

namespace SpinaBets.IntegrationTests
{
    [TestClass]
    public class TransactionIntegrationTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task Deposit_Should_Increase_Balance_And_Save_Transaction()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                AccountId = 1,
                Balance = 1000m,
                IsClosed = false
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            var transaction = new Transaction
            {
                AccountId = 1,
                TransactionType = TransactionType.Deposit,
                Amount = 500m,
                TransactionDate = DateTime.Today,
                Reference = "Deposit Test"
            };

            // Act
            await service.AddTransactionAsync(transaction);

            // Assert
            Assert.AreEqual(1500m, account.Balance);
            Assert.AreEqual(1, context.Transactions.Count());
        }

        [TestMethod]
        public async Task Withdrawal_Should_Decrease_Balance()
        {
            var context = GetDbContext();

            var account = new Account
            {
                AccountId = 1,
                Balance = 1000m,
                IsClosed = false
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            var transaction = new Transaction
            {
                AccountId = 1,
                TransactionType = TransactionType.Withdrawal,
                Amount = 300m,
                TransactionDate = DateTime.Today,
                Reference = "Withdrawal Test"
            };

            await service.AddTransactionAsync(transaction);

            Assert.AreEqual(700m, account.Balance);
        }

        [TestMethod]
        public async Task Withdrawal_With_Insufficient_Funds_Should_Throw()
        {
            var context = GetDbContext();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 100m,
                IsClosed = false
            });

            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.AddTransactionAsync(new Transaction
                {
                    AccountId = 1,
                    TransactionType = TransactionType.Withdrawal,
                    Amount = 500m,
                    TransactionDate = DateTime.Today
                }));
        }

        [TestMethod]
        public async Task Closed_Account_Should_Not_Allow_Transactions()
        {
            var context = GetDbContext();

            context.Accounts.Add(new Account
            {
                AccountId = 1,
                Balance = 1000m,
                IsClosed = true
            });

            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            await Assert.ThrowsAsync<Exception>(() =>
                service.AddTransactionAsync(new Transaction
                {
                    AccountId = 1,
                    TransactionType = TransactionType.Deposit,
                    Amount = 200m,
                    TransactionDate = DateTime.Today
                }));
        }

        [TestMethod]
        public async Task GetByAccount_Should_Return_All_Transactions()
        {
            var context = GetDbContext();

            context.Transactions.AddRange(
                new Transaction
                {
                    AccountId = 1,
                    Amount = 100,
                    TransactionType = TransactionType.Deposit,
                    TransactionDate = DateTime.Today
                },
                new Transaction
                {
                    AccountId = 1,
                    Amount = 50,
                    TransactionType = TransactionType.Withdrawal,
                    TransactionDate = DateTime.Today
                });

            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            var transactions = await service.GetByAccountAsync(1);

            Assert.AreEqual(2, transactions.Count);
        }
    }
}