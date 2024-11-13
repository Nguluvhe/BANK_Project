using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Data
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersWithAccountsAsync();
        Task<IEnumerable<Customer>> FindAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> GetByEmailAsync(string email);
    }
}
