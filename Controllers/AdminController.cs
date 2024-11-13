using UFS_BANK_FINAL.Models;
using UFS_BANK_FINAL.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UFS_BANK_FINAL.Data;
using System.Data;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace UFS_BANK_FINAL.Controllers
{
    [Authorize(Roles = "Admin, Consultant")]
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BankDbContext _context;
        private const string _role = "Client";

        public AdminController(IRepositoryWrapper repositoryWrapper,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            BankDbContext context)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> CustomerList()
        {
            var customers = await _repositoryWrapper.Customer.GetCustomersWithAccountsAsync();
            return View(customers);
        }

        public async Task<IActionResult> CustomerAdvice()
        {
            var accounts = await _context.Accounts.Include(a => a.Customer).ToListAsync();
            return View(accounts);
        }


        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _repositoryWrapper.Customer.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var accounts = await _repositoryWrapper.Account.GetByCustomerIdAsync(id);
            foreach (var account in accounts)
            {
                var transactions = await _repositoryWrapper.Transaction.GetByAccountId(account.AccountNumber);
                foreach (var transaction in transactions)
                {
                    _repositoryWrapper.Transaction.Delete(transaction);
                }
                _repositoryWrapper.Account.Delete(account);
            }
            var user = await _userManager.FindByEmailAsync(customer.Email);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Failed to delete user from Identity tables.");
                }
            }

            _repositoryWrapper.Customer.Delete(customer);
            await _repositoryWrapper.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id, string accountNumber)
        {
            var customer = await _context.Customers
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewData["AccountNumber"] = accountNumber;
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _repositoryWrapper.Customer.Update(customer); 
                await _repositoryWrapper.SaveAsync(); 

                return RedirectToAction(nameof(CustomerList)); 
            }
            return View(customer); 
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateModel createModel)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.FindByNameAsync(createModel.Role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(createModel.Role));
                }

                var user = new IdentityUser
                {
                    UserName = createModel.UserName,
                    Email = createModel.Email
                };

                var result = await _userManager.CreateAsync(user, createModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, createModel.Role); 

                    var customer = new Customer
                    {
                        CustomerName = createModel.CustomerName,
                        Address = createModel.Address,
                        Email = createModel.Email,
                        City = createModel.City,
                        PostCode = createModel.PostCode,
                        Role = createModel.Role
                    };

                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();

                    var account = new Account
                    {
                        AccountNumber = createModel.AccountNumber,
                        AccountType = (char)AccountType.Checking, 
                        Balance = createModel.InitialBalance,
                        ModifyDate = DateTime.Now,
                        UserId = createModel.UserName,
                        CustomerName = createModel.CustomerName,
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
            return View(createModel);
        }
    }


}
