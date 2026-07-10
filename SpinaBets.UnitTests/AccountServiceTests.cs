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
    public class AccountServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [TestMethod]
        public async Task CreateAccount_Should_Create_New_Account()
        {
            // Arrange
            var context = GetDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                UserName = "test@test.com",
                Email = "test@test.com",
                FirstName = "John",
                Surname = "Smith",
                IDNumber = "9901011234567"
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var service = new AccountService(context);

            // Act
            var account = await service.CreateAccountAsync(
                user.Id,
                AccountType.General);

            // Assert
            account.Should().NotBeNull();

            account.UserId.Should().Be(user.Id);

            account.AccountType.Should().Be(AccountType.General);

            account.Balance.Should().Be(0);

            account.IsClosed.Should().BeFalse();

            account.AccountNumber.Should().StartWith("ACC-");

            context.Accounts.Count().Should().Be(1);
        }

        [TestMethod]
        public async Task CloseAccount_Should_Throw_When_Balance_Not_Zero()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-10001",
                AccountType = AccountType.General,
                Balance = 500,
                IsClosed = false
            };

            context.Accounts.Add(account);

            await context.SaveChangesAsync();

            var service = new AccountService(context);

            // Act
            Func<Task> action = async () =>
                await service.CloseAccountAsync(account.AccountId);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Cannot close account with non-zero balance.");
        }

        [TestMethod]
        public async Task CloseAccount_Should_Close_Account_When_Balance_Is_Zero()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-20001",
                AccountType = AccountType.General,
                Balance = 0,
                IsClosed = false
            };

            context.Accounts.Add(account);

            await context.SaveChangesAsync();

            var service = new AccountService(context);

            // Act
            var result = await service.CloseAccountAsync(account.AccountId);

            // Assert
            result.Should().BeTrue();

            account.IsClosed.Should().BeTrue();
        }

        [TestMethod]
        public async Task ReopenAccount_Should_Reopen_Closed_Account()
        {
            // Arrange
            var context = GetDbContext();

            var account = new Account
            {
                UserId = "user1",
                AccountNumber = "ACC-30001",
                AccountType = AccountType.General,
                Balance = 0,
                IsClosed = true
            };

            context.Accounts.Add(account);

            await context.SaveChangesAsync();

            var service = new AccountService(context);

            // Act
            var result = await service.ReopenAccountAsync(account.AccountId);

            // Assert
            result.Should().BeTrue();

            account.IsClosed.Should().BeFalse();
        }

        [TestMethod]
        public async Task CreateAccount_Should_Throw_When_User_Already_Has_Same_Account_Type()
        {
            // Arrange
            var context = GetDbContext();

            var user = new ApplicationUser
            {
                Id = "user1",
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "John",
                Surname = "Smith",
                IDNumber = "9901011234567"
            };

            context.Users.Add(user);

            context.Accounts.Add(new Account
            {
                UserId = user.Id,
                AccountType = AccountType.General,
                AccountNumber = "ACC-11111"
            });

            await context.SaveChangesAsync();

            var service = new AccountService(context);

            // Act
            Func<Task> action = async () =>
                await service.CreateAccountAsync(
                    user.Id,
                    AccountType.General);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("User already has a General account.");
        }

        [TestMethod]
        public async Task CreateAccount_Should_Throw_When_User_Does_Not_Exist()
        {
            // Arrange
            var context = GetDbContext();

            var service = new AccountService(context);

            // Act
            Func<Task> action = async () =>
                await service.CreateAccountAsync(
                    "invalid-user",
                    AccountType.General);

            // Assert
            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("User not found.");
        }
    }
}
