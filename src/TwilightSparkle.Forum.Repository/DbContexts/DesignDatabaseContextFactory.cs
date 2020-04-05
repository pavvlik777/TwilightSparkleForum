using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TwilightSparkle.Forum.Repository.DbContexts
{
    public class DesignDatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB";


        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlServer(ConnectionString);

            return new DatabaseContext(builder.Options);
        }
    }
}