using Microsoft.AspNetCore.Mvc;
using UFS_BANK_FINAL.Data;
using UFS_BANK_FINAL.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UFS_BANK_FINAL.Controllers
{
    public class ConsultantController : Controller
    {
        private readonly BankDbContext _context;

        public ConsultantController(BankDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CustomerFeedBack()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Account)
                .ToListAsync();

            return View(feedbacks);
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
