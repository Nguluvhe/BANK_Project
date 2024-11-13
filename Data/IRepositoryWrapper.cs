namespace UFS_BANK_FINAL.Data
{
    public interface IRepositoryWrapper
    {
        ICustomerRepository Customer { get; }
        IAccountRepository Account { get; }
        ITransactionRepository Transaction { get; }
        void Save();
        Task SaveAsync();
    }
}
