using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpinaBets.DTO;
using SpinaBets.Models;
using SpinaBets.Services;
using SpinaBets.Services.Interfaces;


namespace SpinaBets.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            IAccountService accountService,
            SignInManager<ApplicationUser> signInManager)

        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._accountService = accountService;
        }
        #region Profile
        public async Task<IActionResult> Index()
        {
            var user=await _userManager.GetUserAsync(User);

            var accounts = await _accountService.GetUserAccountsAsync(user.Id);

            return View(accounts);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            AccountType type)
        {
            var user = await _userManager.GetUserAsync(User);

            try
            {
                await _accountService
                    .CreateAccountAsync(user.Id, type);

                return RedirectToAction(nameof(MyAccounts));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Close(int id)
        {
            try
            {
                await _accountService.CloseAccountAsync(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details),
                new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Reopen(int id)
        {
            await _accountService.ReopenAccountAsync(id);

            return RedirectToAction(nameof(Details),
                new { id });
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var account =
                await _accountService.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [Authorize]
        public async Task<IActionResult> MyAccounts()
        {
            var user = await _userManager.GetUserAsync(User);

            var accounts = await _accountService
                .GetUserAccountsAsync(user.Id);

            return View(accounts);
        }
        #endregion


        #region Authentication
        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            //Check duplicate id number
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u =>
                    u.IDNumber == registerDto.IDNumber);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "",
                    "ID Number already exists.");

                return View(registerDto);
            }

            // CREATE USER
            var user = new ApplicationUser
            {
                FirstName = registerDto.FirstName,
                Surname = registerDto.Surname,
                IDNumber = registerDto.IDNumber,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager
                .CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // ADD CUSTOMER ROLE
                await _userManager.AddToRoleAsync(
                    user,
                    "Customer");

                // SIGN IN USER
                await _signInManager.SignInAsync(
                    user,
                    false);

                return RedirectToAction(
                    "Customer",
                    "Dashboard");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }

            return View(registerDto);
        }

       

        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var result =
                await _signInManager.PasswordSignInAsync(
                    loginDto.Email,
                    loginDto.Password,
                    loginDto.RememberMe,
                    false);

            if (result.Succeeded)
            {
                var user = await _userManager
                    .FindByEmailAsync(loginDto.Email);

                if (await _userManager.IsInRoleAsync(
                    user,
                    "Admin"))
                {
                    return RedirectToAction(
                        "Admin",
                        "Home");
                }

                return RedirectToAction(
                    "Customer",
                    "Home");
            }

            ViewBag.ErrorMessage =
                "Invalid login attempt";

            return View(loginDto);
        }

      

        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return RedirectToAction(
                "Index",
                "Home");
        }
        #endregion


    }
}
