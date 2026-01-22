using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Data.Repositories
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class
    {
        private readonly ShopperDBContext _dbContext;
        private DbSet<T> _dbSet;
        
        public CommonRepository(ShopperDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task CreateAsync(T dbrecord)
        {
            await _dbSet.AddAsync(dbrecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();
        }
    }
}
