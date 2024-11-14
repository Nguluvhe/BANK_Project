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
    }
}
