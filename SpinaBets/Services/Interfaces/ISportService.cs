using SpinaBets.Models;

namespace SpinaBets.Services.Interfaces
{
    public interface ISportService
    {
        Task<List<Sport>> GetAllAsync();

        Task<Sport?> GetByIdAsync(int id);

        Task CreateAsync(Sport sport);

        Task UpdateAsync(Sport sport);

        Task DeleteAsync(int id);
    }
}
