using UFS_BANK_FINAL.Models;
using Microsoft.EntityFrameworkCore;

namespace UFS_BANK_FINAL.Data
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        private readonly BankDbContext _context;
        public CustomerRepository(BankDbContext bankDbContext) : base(bankDbContext)
        {
            _context = bankDbContext;
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithAccountsAsync()
        {
            return await _bankDbContext.Customers.Include(c => c.Accounts).ToListAsync(); 
        }
        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
        public async Task<IEnumerable<Customer>> FindAllAsync()
        {
            return await _bankDbContext.Customers.ToListAsync();
        }
        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<Customer>> FindCustomerById()
        {
            return await _context.Customers.ToListAsync();
        }

        public void AddCustomers(Customer entity)
        {
            _context.Customers.Add(entity);
        }

        public void UpdateCustomers(Customer entity)
        {
            _context.Customers.Update(entity);
        }

        public void DeleteCustomers(Customer entity)
        {
            _context.Customers.Remove(entity);
        }
        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
