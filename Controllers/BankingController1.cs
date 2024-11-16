using Microsoft.AspNetCore.Mvc;
using UFS_BANK_FINAL.Data;
using UFS_BANK_FINAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace UFS_BANK_FINAL.Controllers
{
    [Authorize(Roles = "Client")]
    public class BankingController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly BankDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BankingController(IRepositoryWrapper repositoryWrapper,
            BankDbContext context, UserManager<IdentityUser> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _context = context;
            _userManager = userManager;
        }

        // GET: Deposit
        public async Task<IActionResult> Deposit()
        {
            // Get the logged-in user by their username (this ensures the user is authenticated)
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return View();
            }

            // Find the account associated with the user
            var userAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == user.UserName);

            if (userAccount == null)
            {
                TempData["ErrorMessage"] = "Account not found for the current user.";
                return View();
            }

            // Pass the account number to the view so it can be populated in the form
            ViewData["AccountNumber"] = userAccount.AccountNumber;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Deposit(int accountNumber, double amount)
        {
            var account = await _context.Accounts.FindAsync(accountNumber);
            if (account == null)
            {
                ModelState.AddModelError("", "Account not found.");
                return View();
            }

            account.Balance += amount;
            account.ModifyDate = DateTime.Now;

            var transaction = new Transaction
            {
                TransactionType = TransactionType.Deposit,
                AccountNumber = accountNumber,
                Amount = amount,
                ModifyDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Deposit of {amount:C} to account {accountNumber} was successful.";

            return RedirectToAction("Deposit", "Banking");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var customer = await _context.Customers
                                         .Include(c => c.Accounts)
                                 .FirstOrDefaultAsync(c => c.Accounts.Any(a => a.UserId == user.UserName));

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile));
            }

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "The new password and confirmation password do not match.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                TempData["Message"] = "Password changed successfully!";
                return RedirectToAction("Profile");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Transactions(int accountNumber)
        {
            // Fetch the transactions for the provided account number
            var transactions = await _context.Transactions
                .Where(t => t.Account.AccountNumber == accountNumber) 
                .ToListAsync();

            return View(transactions);
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Transfer(int fromAccountNumber, int toAccountNumber, double amount)
        {
            var fromAccount = await _context.Accounts.FindAsync(fromAccountNumber);
            var toAccount = await _context.Accounts.FindAsync(toAccountNumber);

            if (fromAccount == null || toAccount == null)
            {
                ModelState.AddModelError("", "One or both accounts not found.");
                return View();
            }

            if (fromAccount.Balance < amount)
            {
                ModelState.AddModelError("", "Insufficient funds.");
                return View();
            }

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            fromAccount.ModifyDate = DateTime.Now;
            toAccount.ModifyDate = DateTime.Now;

            var transactionFrom = new Transaction
            {
                TransactionType = TransactionType.Transfer,
                AccountNumber = fromAccountNumber,
                DestAccount = toAccountNumber,
                Amount = -amount,
                ModifyDate = DateTime.Now
            };

            var transactionTo = new Transaction
            {
                TransactionType = TransactionType.Transfer,
                AccountNumber = toAccountNumber,
                DestAccount = fromAccountNumber,
                Amount = amount,
                ModifyDate = DateTime.Now
            };

            _context.Transactions.Add(transactionFrom);
            _context.Transactions.Add(transactionTo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully transferred {amount:C} from account {fromAccountNumber} to account {toAccountNumber}.";

            return RedirectToAction("Transfer", "Banking");
        }
    }

}

