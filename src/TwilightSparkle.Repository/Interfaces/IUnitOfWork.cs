using System;
using System.Threading.Tasks;

namespace TwilightSparkle.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        Task SaveAsync();
    }
}