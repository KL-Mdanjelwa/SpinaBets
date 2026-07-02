using SpinaBets.DbContext;
using SpinaBets.DTO;
using SpinaBets.Models;
using SpinaBets.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;

namespace SpinaBets.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardReport?> GetAdminDashboardAsync()
        {
            return await _context.AdminDashboardReport
                .FirstOrDefaultAsync();
        }

        public async Task<List<BettingReportDto>> GetBettingReportAsync()
        {
            return await _context.BettingReports
            .FromSqlRaw("EXEC sp_GetBettingReport")
            .ToListAsync();
        }
    }
  }
