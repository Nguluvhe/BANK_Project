namespace UFS_BANK_FINAL.Data
{
    public interface IRepositoryBase<T>
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> FindAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

}
