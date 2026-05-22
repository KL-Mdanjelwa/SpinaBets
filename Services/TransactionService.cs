using SpinaBets.Models;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace SpinaBets.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetByAccountAsync(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

      
        public async Task AddTransactionAsync(Transaction transaction)
        {
            Console.WriteLine("POST HIT");
            var account = await _context.Accounts
               .FirstOrDefaultAsync(a => a.AccountId == transaction.AccountId);

            if (account == null)
                throw new Exception("Account not found.");

            if (account.IsClosed)
                throw new Exception("No transactions allowed on closed account.");

            if (transaction.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            if (transaction.TransactionDate > DateTime.Today)
                throw new Exception("Date cannot be in future.");


            if (transaction.TransactionType == TransactionType.Withdrawal)
            {
                if (account.Balance < transaction.Amount)
                    throw new Exception("Insufficient balance.");

                account.Balance -= transaction.Amount;
            }
            else
            {
                account.Balance += transaction.Amount;
            }

            transaction.CaptureDate = DateTime.Now;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task EditTransactionAsync(Transaction transaction)
        {
            var existing = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);

            if (existing == null)
                throw new Exception("Transaction not found.");

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == existing.AccountId);

            if (account == null)
                throw new Exception("Account not found.");

            if (account.IsClosed)
                throw new Exception("Closed account.");

            if (transaction.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            if (transaction.TransactionDate > DateTime.Today)
                throw new Exception("Future date not allowed.");

            // REVERSE OLD TRANSACTION EFFECT
            if (existing.TransactionType == TransactionType.Withdrawal)
                account.Balance += existing.Amount;
            else
                account.Balance -= existing.Amount;

            // APPLY NEW TRANSACTION EFFECT
            if (existing.TransactionType == TransactionType.Withdrawal)
            {
                if (account.Balance < transaction.Amount)
                    throw new Exception("Insufficient balance.");

                account.Balance -= transaction.Amount;
            }
            else
            {
                account.Balance += transaction.Amount;
            }

            // UPDATE FIELDS
            existing.Amount = transaction.Amount;
            existing.TransactionType = transaction.TransactionType;
            existing.TransactionDate = transaction.TransactionDate;
            existing.Reference = transaction.Reference;
            existing.CaptureDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
