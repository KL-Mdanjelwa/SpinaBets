using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services.Interfaces;

namespace SpinaBets.Controllers
{
    [Authorize] 
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ISportService _sportService;

        public GamesController(
            IGameService gameService,
            ISportService sportService)
        {
            _gameService = gameService;
            _sportService = sportService;
        }


        public async Task<IActionResult> Index()
        {
            Console.WriteLine("GAMES INDEX CONTROLLER HIT");
            var games = await _gameService.GetAll();
            return View(games);
        }
        public async Task<IActionResult> SetResult(int id)
        {
            var game = await _gameService.GetById(id);

            if (game == null)
                return NotFound();

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetResult(int id, string result)
        {
            try
            {
                await _gameService.SetResult(id, result);

                TempData["Success"] = "Game settled successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Sports = await _sportService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Sports = await _sportService.GetAllAsync();
                return View(game);
            }

            await _gameService.Create(game);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetById(id);

            if (game == null)
                return NotFound();

            ViewBag.Sports = await _sportService.GetAllAsync();

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Game game)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Sports = await _sportService.GetAllAsync();
                return View(game);
            }

            await _gameService.Update(game);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _gameService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
