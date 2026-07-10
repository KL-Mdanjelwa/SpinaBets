using SpinaBets.Models;

namespace SpinaBets.Services.Interfaces
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(Transaction transaction);

        Task EditTransactionAsync(Transaction transaction);

        Task<List<Transaction>> GetByAccountAsync(int accountId);
    }
}
