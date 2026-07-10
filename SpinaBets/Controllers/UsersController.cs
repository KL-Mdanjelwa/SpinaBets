using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DbContext;
using SpinaBets.Models;
using SpinaBets.ViewModels;


namespace SpinaBets.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }




        public async Task<IActionResult> Index(string search = "", int page = 1)
        {
            const int pageSize = 5;

            var users = _userManager.Users.AsQueryable();

            // Search by ID Number or Surname
            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u =>
                    u.Surname.Contains(search) ||
                    u.IDNumber.Contains(search));
            }

            var accounts = await _context.Accounts.ToListAsync();

            var userList = await users.ToListAsync();

            var model = new List<UserViewModel>();

            foreach (var user in userList)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var account = accounts.FirstOrDefault(a => a.UserId == user.Id);

                // Search by Account Number
                if (!string.IsNullOrWhiteSpace(search))
                {
                    if (!(user.Surname.Contains(search,
                            StringComparison.OrdinalIgnoreCase)
                        || user.IDNumber.Contains(search)
                        || (account != null &&
                            account.AccountNumber.Contains(search,
                            StringComparison.OrdinalIgnoreCase))))
                    {
                        continue;
                    }
                }

                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber,
                    IDNumber = user.IDNumber,
                    Roles = roles.ToList(),
                    CreatedDate = user.CreatedDate,
                    AccountNumber = account?.AccountNumber ?? "-"
                });
            }

            var totalUsers = model.Count;

            var pagedUsers = model
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new UsersIndexViewModel
            {
                Users = pagedUsers,
                Search = search,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize)
            };

            return View(vm);
        }


        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IDNumber = user.IDNumber,
                Roles = roles.ToList(),
                CreatedDate = user.CreatedDate
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var model = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IDNumber = user.IDNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.PhoneNumber = model.PhoneNumber;
            user.IDNumber = model.IDNumber;

            // Email update (IMPORTANT Identity method)
            var emailResult = await _userManager.SetEmailAsync(user, model.Email);

            if (!emailResult.Succeeded)
            {
                foreach (var error in emailResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
