using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Repository.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;


        protected DbSet<T> Entities => _dbContext.Set<T>();


        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<IReadOnlyCollection<T>> GetWhereAsync(Expression<Func<T, bool>> filter)
        {
            return await Entities.Where(filter).ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await Entities.FirstOrDefaultAsync(filter);
        }

        public void Create(T item)
        {
            Entities.Add(item);
        }

        public void Update(T item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(T item)
        {
            Entities.Remove(item);
        }
    }
}