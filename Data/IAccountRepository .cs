using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Data
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(string userId);
        Task<IEnumerable<Account>> GetByCustomerIdAsync(int customerId);
    }
}
