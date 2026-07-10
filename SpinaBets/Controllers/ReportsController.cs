using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpinaBets.Services.Interfaces;


namespace SpinaBets.Controllers
{
    namespace SpinaBets.Controllers
    {
        [Authorize(Roles = "Admin")]
        public class ReportsController : Controller
        {
            private readonly IReportService _reportService;

            public ReportsController(IReportService reportService)
            {
                _reportService = reportService;
            }

            public async Task<IActionResult> Index()
            {
                var report = await _reportService.GetBettingReportAsync();

                return View(report);
            }

            public async Task<IActionResult> Dashboard()
            {
                var dashboard =
                    await _reportService.GetAdminDashboardAsync();

                return View(dashboard);
            }
        }
    }
 }
