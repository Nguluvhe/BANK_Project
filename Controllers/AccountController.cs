using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: ForgotPassword
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["InfoMessage"] = "If an account with this email exists, a reset link has been sent.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var resetModel = new ResetPasswordModel
            {
                Email = user.Email,
                Token = encodedToken
            };

            return View("ForgotPasswordConfirmation", resetModel);
        }


        // GET: ForgotPasswordConfirmation
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: ResetPassword
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid password reset token or email.");
            }

            var model = new ResetPasswordModel { Email = email, Token = token };
            return View(model);
        }


        // POST: ResetPassword
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["InfoMessage"] = "Password reset failed. Please try again.";
                return RedirectToAction("ForgotPassword");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your password has been reset successfully.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
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

