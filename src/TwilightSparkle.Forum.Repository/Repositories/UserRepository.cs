using Microsoft.EntityFrameworkCore;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;
using TwilightSparkle.Repository.Implementations;

namespace TwilightSparkle.Forum.Repository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}