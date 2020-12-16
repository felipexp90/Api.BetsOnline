using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Providers
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public void Add(T entity) => _unitOfWork.Context.Set<T>().Add(entity);

        public void Update(T entity) => _unitOfWork.Context.Entry(entity).State = EntityState.Modified;

        public async Task<T> Get(int id) => await _unitOfWork.Context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> filter = null, int page = 1, int pageSize = 20,
            params Expression<Func<T, object>>[] includeProperties)
        {
            if (page < 1) throw new ApplicationException("The Page must be greater than zero");
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = orderBy(query);
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> List(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            var item = await query.FirstOrDefaultAsync(where);
            return item;
        }

        public async Task Delete(int id)
        {
            T existing = await _unitOfWork.Context.Set<T>().FindAsync(id);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.AnyAsync(where); ;
        }

        public IQueryable<T> AsQueryable() => _unitOfWork.Context.Set<T>().AsQueryable();

        private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>> includeProperties, IQueryable<T> query) => includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        public async Task<T> Get(string guid) => await _unitOfWork.Context.Set<T>().FindAsync(guid);
    }
}