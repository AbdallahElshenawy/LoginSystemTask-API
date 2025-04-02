using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? criteria = null, int skip = 0,
            int take = int.MaxValue, string[]? includes = null,
            Expression<Func<T, object>>? orderBy = null, OrderByDirection orderByDirection = OrderByDirection.Ascending);
        
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }

    public enum OrderByDirection
    {
        Ascending,
        Descending
    }
}
