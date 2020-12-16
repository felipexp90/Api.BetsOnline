using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<T> Get(string guid);
        Task<bool> Any(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> filter = null, int page = 1, int pageSize = 20, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        Task Delete(int id);
        void Update(T entity);
    }
}