using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Repositories
{
    public class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
    {
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? criteria = null, int skip = 0,
            int take = int.MaxValue, string[]? includes = null,  Expression<Func<T, object>>? orderBy = null,
            OrderByDirection orderByDirection = OrderByDirection.Ascending)
        {
            IQueryable<T> query = context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            if (orderBy != null)
            {
                query = orderByDirection == OrderByDirection.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            return await query.Skip(skip).Take(take).ToListAsync();
        }
        public async Task<T?> GetByIdAsync(Guid id) => await context.Set<T>().FindAsync(id);
        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            return await context.SaveChangesAsync() > 0;

        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            context.Set<T>().Remove(entity);
            return await context.SaveChangesAsync() > 0;

        }
    }
}
