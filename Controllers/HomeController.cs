using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UFS_BANK_FINAL.Data;
using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Controllers
{

    public class HomeController : Controller
    {
        private readonly BankDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(BankDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous] // Allow access to this action even when the user is not authenticated
        public IActionResult Error()
        {
            // You can retrieve the exception message here if you want to display it
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionDetails != null)
            {
                var exception = exceptionDetails.Error;
                // You can log the exception here if needed
            }

            // Return the Error view
            return View();
        }

        public async Task<IActionResult> Index()
        {
            Account accountModel = null;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    accountModel = await _context.Accounts
                                      .Include(a => a.Customer)
                                      .FirstOrDefaultAsync(a => a.UserId == user.UserName);
                }
            }

            return View(accountModel);
        }

    }
}
