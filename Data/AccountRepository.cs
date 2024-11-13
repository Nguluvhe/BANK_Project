using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankDbContext _context;

        public AccountRepository(BankDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId)
        {
            return await _context.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync(); // Ensure you have the necessary using directive
        }
        public async Task<IEnumerable<Account>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Accounts
                                 .Where(account => account.Customer.CustomerID == customerId)
                                 .ToListAsync();  // Assuming your Account model has a CustomerId property
        }

        // Implementing methods from IRepositoryBase<Account>
        public async Task<Account> GetById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<Account>> FindAll()
        {
            return await _context.Accounts.ToListAsync();
        }

        public void Add(Account entity)
        {
            _context.Accounts.Add(entity);
        }

        public void Update(Account entity)
        {
            _context.Accounts.Update(entity);
        }

        public void Delete(Account entity)
        {
            _context.Accounts.Remove(entity);
        }
    }
}
