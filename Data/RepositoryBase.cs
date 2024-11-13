using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace UFS_BANK_FINAL.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected BankDbContext _bankDbContext;

        public RepositoryBase(BankDbContext bankDbContext)
        {
            _bankDbContext = bankDbContext;
        }

        public void Add(T entity)
        {
            _bankDbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _bankDbContext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _bankDbContext.Set<T>().ToListAsync(); // Ensure you are returning Task<IEnumerable<T>>
        }

        public async Task<T> GetById(int id)
        {
            return await _bankDbContext.Set<T>().FindAsync(id); // Ensure you are returning Task<T>
        }

        public void Update(T entity)
        {
            _bankDbContext.Set<T>().Update(entity);
        }
    }
}
