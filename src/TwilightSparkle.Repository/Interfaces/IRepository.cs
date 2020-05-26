using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TwilightSparkle.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyCollection<T>> GetWhereAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);

        void Create(T item);

        void Update(T item);

        void Delete(T item);
    }
}