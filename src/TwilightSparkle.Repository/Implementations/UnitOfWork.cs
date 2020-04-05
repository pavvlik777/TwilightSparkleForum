using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        private readonly Dictionary<Type, Type> _customRepositoryTypes;
        private readonly Dictionary<Type, object> _repositories;


        public UnitOfWork(DbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            _customRepositoryTypes = new Dictionary<Type, Type>();
            _repositories = new Dictionary<Type, object>();
        }


        public IRepository<T> GetRepository<T>() where T : class
        {
            var entityType = typeof(T);
            if (_repositories.TryGetValue(entityType, out var repository))
            {
                return (IRepository<T>)repository;
            }

            var newRepository = CreateNewRepository<T>();
            _repositories.Add(entityType, newRepository);

            return newRepository;
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        protected void RegisterCustomRepository<TEntity, TRepository>()
        {
            _customRepositoryTypes.Add(typeof(TEntity), typeof(TRepository));
        }


        private IRepository<T> CreateNewRepository<T>() where T : class
        {
            var entityType = typeof(T);
            var newRepository = _customRepositoryTypes.TryGetValue(entityType, out var repositoryType)
                ? Activator.CreateInstance(repositoryType, _context)
                : new Repository<T>(_context);

            return (IRepository<T>)newRepository;
        }
    }
}