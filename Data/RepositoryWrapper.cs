namespace UFS_BANK_FINAL.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly BankDbContext _bankDbContext;
        private ICustomerRepository _customer;
        private IAccountRepository _account;
        private ITransactionRepository _transaction;

        public RepositoryWrapper(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }

        public ICustomerRepository Customer
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerRepository(_bankDbContext);
                }
                return _customer;
            }
        }

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_bankDbContext);
                }
                return _account;
            }
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_bankDbContext);
                }
                return _transaction;
            }
        }

        public void Save()
        {
            _bankDbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _bankDbContext.SaveChangesAsync(); 
        }
    }
}
