using Microsoft.EntityFrameworkCore;
using TwilightSparkle.Forum.DomainModel.Entities;

namespace TwilightSparkle.Forum.Repository.DbContexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}