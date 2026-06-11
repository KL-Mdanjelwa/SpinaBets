using SpinaBets.Models;

namespace SpinaBets.Services.Interfaces
{
    public interface IBetService
    {
        Task PlaceBetAsync(Bet bet);

        Task<List<Bet>> GetByAccountAsync(int accountId);

        Task<Bet?> GetByIdAsync(int betId);

        Task SettleBetAsync(int betId, string result);
    }
}
