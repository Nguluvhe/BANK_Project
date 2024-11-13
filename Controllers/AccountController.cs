using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UFS_BANK_FINAL.Data;
using UFS_BANK_FINAL.Models;
using UFS_BANK_FINAL.Models.ViewModels;

namespace UFS_BANK_FINAL.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BankDbContext _context;
        private const string _role = "Client";
        public AccountController(IRepositoryWrapper repositoryWrapper,
            UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, BankDbContext context)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateModel registeModel)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.FindByNameAsync(_role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(_role));
                }

                var user = new IdentityUser
                {
                    UserName = registeModel.UserName,
                    Email = registeModel.Email
                };

                var result = await _userManager.CreateAsync(user, registeModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, _role); 

                    var customer = new Customer
                    {
                        CustomerName = registeModel.CustomerName,
                        Address = registeModel.Address,
                        Email = registeModel.Email,
                        City = registeModel.City,
                        PostCode = registeModel.PostCode,
                        Role = _role
                    };

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    var account = new Account
                    {
                        AccountNumber = registeModel.AccountNumber,
                        AccountType = (char)AccountType.Checking,
                        Balance = registeModel.InitialBalance,
                        ModifyDate = DateTime.Now,
                        UserId = registeModel.UserName,
                        CustomerName = registeModel.CustomerName,
                        Customer = customer
                    };

                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync(); 

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(registeModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user =
                  await _userManager.FindByNameAsync(loginModel.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,
                      loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

