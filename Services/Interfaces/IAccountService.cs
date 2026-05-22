using SpinaBets.Models;

namespace SpinaBets.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<Account>> GetUserAccountsAsync(string userId);
        Task<Account> CreateAccountAsync(string userId, AccountType type);
        //Task<Account> DeleteUserAsync(string userId);

        Task<bool> CloseAccountAsync(int accountId);

        Task<bool> ReopenAccountAsync(int accountId);

        Task<Account> GetByIdAsync(int accountId);
    }
}
