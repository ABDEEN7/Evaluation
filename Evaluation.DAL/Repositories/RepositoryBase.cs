using Evaluation.DAL.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Evaluation.DAL.Repositories
{
    public abstract class RepositoryBase<T>(DbContext context, UserInfo userInfo) where T : class
    {
        protected readonly DbContext context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? pageNumber = 0, int? pageSize = 0, params Expression<Func<T, object>>?[]? includeProperties)
        {
            IQueryable<T> query = _dbSet.IgnoreQueryFilters();

            if (includeProperties != null)
                foreach (var item in includeProperties)
                    if (item != null)
                        query = query.Include(item);

            if (filter != null)
                query = query.Where(filter);

            query = orderBy != null ? orderBy(query) : query;

            if (pageNumber > 0 && pageSize > 0)
                query = query.Skip((pageNumber!.Value - 1) * pageSize!.Value).Take(pageSize!.Value);

            return query;
        }
    }
}
