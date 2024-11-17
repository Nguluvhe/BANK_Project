using Microsoft.AspNetCore.Authorization;
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
        //public async Task<IActionResult> Index()
        //{
        //    Account account = null;
        //    //if (!User.Identity.IsAuthenticated)
        //    //{
        //    //    return RedirectToAction("Login", "Account");
        //    //}

        //    //var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    //var account = await _context.Accounts
        //    //                    .Include(a => a.Customer)
        //    //                    .FirstOrDefaultAsync(a => a.UserId == user.UserName);
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        var account = await _context.Accounts
        //                            .Include(a => a.Customer)
        //                            .FirstOrDefaultAsync(a => a.UserId == user.UserName);
        //    }

        //    return View(account);
        //}
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
