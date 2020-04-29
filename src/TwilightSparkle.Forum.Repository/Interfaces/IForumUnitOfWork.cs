using TwilightSparkle.Repository.Interfaces;

namespace TwilightSparkle.Forum.Repository.Interfaces
{
    public interface IForumUnitOfWork : IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}