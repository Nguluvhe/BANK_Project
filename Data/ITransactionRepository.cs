using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Data
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        Task<IEnumerable<Transaction>> GetByAccountId(int accountId);
    }
}
