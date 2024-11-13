using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UFS_BANK_FINAL.Data;
using Microsoft.EntityFrameworkCore;
using UFS_BANK_FINAL.Models; // Make sure to include your models namespace

namespace UFS_BANK_FINAL.Controllers
{
    [Authorize(Roles = "FinancialAdvisor")]
    public class FinancialAdvisorController : Controller
    {
        private readonly BankDbContext _context;

        public FinancialAdvisorController(BankDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return View(accounts);
        }

        [HttpGet]
        public IActionResult ProvideAdvice(int accountNumber)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            return View(account); 
        }

        [HttpPost]
        public async Task<IActionResult> ProvideAdvice(Account account)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = await _context.Accounts.FindAsync(account.AccountNumber);
                if (existingAccount != null)
                {
                    existingAccount.Advice = account.Advice; 
                    _context.Update(existingAccount);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "You have successfully provided advice.";

                    return RedirectToAction("Index", "FinancialAdvisor");
                }
            }
            return View(account);
        }
    }
}
