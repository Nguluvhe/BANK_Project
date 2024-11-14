using Microsoft.AspNetCore.Mvc;
using UFS_BANK_FINAL.Data;
using UFS_BANK_FINAL.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace UFS_BANK_FINAL.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly BankDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public FeedbackController(BankDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var account = await _context.Accounts
                                .Include(a => a.Customer)
                                .FirstOrDefaultAsync(a => a.UserId == user.UserName);


            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback(string feedbackMessage, int AccountNumber)
        {
            if (string.IsNullOrWhiteSpace(feedbackMessage))
            {
                return BadRequest("Feedback message cannot be empty.");
            }

            var feedback = new Feedback
            {
                AccountNumber = AccountNumber,
                Message = feedbackMessage,
                SubmitDate = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            TempData["FeedbackMessage"] = "Thank you for your feedback!";
            return RedirectToAction("Index", "Home");
        }
    }
}

