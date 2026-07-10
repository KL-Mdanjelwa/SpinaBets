using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace SpinaBets.UnitTests
{
    [TestClass]
    public class TransactionServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task Deposit_Should_Increase_Balance()
        {
            var context = GetDbContext();

            var account = new Account
            {
                AccountNumber = "ACC-1001",
                UserId = "user1",
                Balance = 1000,
                IsClosed = false
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Amount = 500,
                TransactionType = TransactionType.Deposit,
                TransactionDate = DateTime.Today
            };

            await service.AddTransactionAsync(transaction);

            account.Balance.Should().Be(1500);

            context.Transactions.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task Withdrawal_Should_Decrease_Balance()
        {
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-1002",
                Balance = 1000,
                IsClosed = false
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            await service.AddTransactionAsync(new Transaction
            {
                AccountId = account.AccountId,
                Amount = 300,
                TransactionType = TransactionType.Withdrawal,
                TransactionDate = DateTime.Today
            });

            account.Balance.Should().Be(700);
        }

        [TestMethod]
        public async Task Withdrawal_Should_Throw_When_Insufficient_Balance()
        {
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-1003",
                Balance = 100,
                IsClosed = false
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            Func<Task> action = async () =>
                await service.AddTransactionAsync(new Transaction
                {
                    AccountId = account.AccountId,
                    Amount = 500,
                    TransactionType = TransactionType.Withdrawal,
                    TransactionDate = DateTime.Today
                });

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Insufficient balance.");
        }

        [TestMethod]
        public async Task Closed_Account_Should_Not_Allow_Transactions()
        {
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-1004",
                Balance = 500,
                IsClosed = true
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            Func<Task> action = async () =>
                await service.AddTransactionAsync(new Transaction
                {
                    AccountId = account.AccountId,
                    Amount = 100,
                    TransactionType = TransactionType.Deposit,
                    TransactionDate = DateTime.Today
                });

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("No transactions allowed on closed account.");
        }

        [TestMethod]
        public async Task Future_Date_Should_Throw_Exception()
        {
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-1005",
                Balance = 500
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            var service = new TransactionService(context);

            Func<Task> action = async () =>
                await service.AddTransactionAsync(new Transaction
                {
                    AccountId = account.AccountId,
                    Amount = 100,
                    TransactionType = TransactionType.Deposit,
                    TransactionDate = DateTime.Today.AddDays(1)
                });

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Date cannot be in future.");
        }

        

        




    }
}
