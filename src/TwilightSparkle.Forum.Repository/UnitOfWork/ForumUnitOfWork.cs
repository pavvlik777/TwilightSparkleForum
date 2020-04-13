using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;
using TwilightSparkle.Forum.Repository.Repositories;

namespace TwilightSparkle.Forum.Repository.UnitOfWork
{
    public class ForumUnitOfWork : TwilightSparkle.Repository.Implementations.UnitOfWork, IForumUnitOfWork
    {
        public IUserRepository UserRepository => (IUserRepository)GetRepository<User>();


        public ForumUnitOfWork(DbContext dbContext)
            : base(dbContext)
        {
            RegisterCustomRepository<User, UserRepository>();
        }
    }
}