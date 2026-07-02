using SpinaBets.DTO;
using SpinaBets.Models;

namespace SpinaBets.Services.Interfaces
{
    public interface IReportService
    {
        Task<AdminDashboardReport?> GetAdminDashboardAsync();

        Task<List<BettingReportDto>> GetBettingReportAsync();
    }
}
