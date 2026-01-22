using System.Linq.Expressions;

namespace E_Commerce.Data.Repositories
{
    public interface ICommonRepository<T> where T : class
    {
        Task CreateAsync(T dbrecord);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAll();
        Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter);
        Task UpdateAsync();
        Task DeleteAsync(T dbRecord);
    }
}
