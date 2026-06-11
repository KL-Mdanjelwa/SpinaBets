using SpinaBets.Models;
using Microsoft.EntityFrameworkCore;
using SpinaBets.Services.Interfaces;
using SpinaBets.DbContext;

namespace SpinaBets.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

       

        public async Task<Account> GetByIdAsync(int accountId)
        {
            return await _context.Accounts
           .Include(a => a.Transactions)
           .Include(a => a.Bets)
           .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task<Account> CreateAccountAsync(
            string userId,
              AccountType type)
            {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == userId);

            if (!userExists)
            {
                throw new Exception("User not found.");
            }

            var existingAccount = await _context.Accounts
                .AnyAsync(a =>
                    a.UserId == userId &&
                    a.AccountType == type);

            if (existingAccount)
            {
                throw new Exception(
                    $"User already has a {type} account.");
            }
           

                string number = $"ACC-{DateTime.UtcNow.Ticks.ToString()[^8..]}";
            

            var account = new Account
            {
                UserId = userId,
                AccountNumber = number,
                AccountType = type,
                Balance = 0,
                IsClosed = false,
                CreatedDate = DateTime.UtcNow
            };

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<bool> CloseAccountAsync(int accountId)
        {
            Console.WriteLine("CLOSE SERVICE HIT");
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null)
                return false;

            if (account.Balance != 0)
                throw new Exception("Cannot close account with non-zero balance.");

            account.IsClosed = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReopenAccountAsync(int accountId)
        {
            Console.WriteLine("CLOSE SERVICE HIT");
            var account = await _context.Accounts.FindAsync(accountId);

            if (account == null)
                return false;

            account.IsClosed = false;

            await _context.SaveChangesAsync();

            return true;
        }

     
    
        public async Task<List<Account>> GetUserAccountsAsync(string userId)
        {
           
          return await _context.Accounts
         .Include(a => a.Transactions) 
         .Include(a => a.Bets)         
         .Where(a => a.UserId == userId)
         .ToListAsync();
        }

      
        
    }
    }
