using Microsoft.EntityFrameworkCore;
using UFS_BANK_FINAL.Models;

namespace UFS_BANK_FINAL.Data
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankDbContext bankDbContext) : base(bankDbContext)
        {
        }
        public async Task<IEnumerable<Transaction>> GetByAccountId(int accountId)
        {
            return await _bankDbContext.Transactions
                                 .Where(t => t.AccountNumber == accountId)
                                 .ToListAsync();
        }
    }
}
