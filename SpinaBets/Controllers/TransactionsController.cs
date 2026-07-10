using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.Services.Interfaces;


namespace SpinaBets.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _context;

        public TransactionsController(
            ITransactionService transactionService,
            ApplicationDbContext context)
        {
            _transactionService = transactionService;
            _context = context;
        }
        
        public async Task<IActionResult> Index(int accountId)
        {
            var transactions = await _transactionService.GetByAccountAsync(accountId);

            ViewBag.AccountId = accountId;

            return View(transactions);
        }

        [HttpGet]
        public IActionResult Create(int accountId)
        {
            ViewBag.AccountId = accountId;

            return View(new Transaction
            {
                AccountId = accountId,
                TransactionDate = DateTime.Today
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transaction model)
        {
            Console.WriteLine("CONTROLLER HIT");
            Console.WriteLine($"POST TransactionType = {model.TransactionType}");
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"FIELD: {error.Key}");

                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"ERROR: {subError.ErrorMessage}");
                    }
                }
                return View(model);
            }

            try
            {
                await _transactionService.AddTransactionAsync(model);

                return RedirectToAction(
                    "Index",
                    new { accountId = model.AccountId }
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
                return NotFound();

            return View(transaction);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Transaction model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _transactionService.EditTransactionAsync(model);

                return RedirectToAction(
                    "Index",
                    new { accountId = model.AccountId }
                );
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
