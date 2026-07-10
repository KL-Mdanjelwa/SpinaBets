using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpinaBets.Models;
using SpinaBets.Services.Interfaces;

namespace SpinaBets.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SportsController : Controller
    {
        private readonly ISportService _sportService;

        public SportsController(ISportService sportService)
        {
            _sportService = sportService;
        }

        public async Task<IActionResult> Index()
        {
            var sports = await _sportService.GetAllAsync();
            return View(sports);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sport sport)
        {
            if (!ModelState.IsValid)
                return View(sport);

            await _sportService.CreateAsync(sport);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sport = await _sportService.GetByIdAsync(id);

            if (sport == null)
                return NotFound();

            return View(sport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Sport sport)
        {
            if (!ModelState.IsValid)
                return View(sport);

            await _sportService.UpdateAsync(sport);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sportService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
