using Microsoft.EntityFrameworkCore;
using TwilightSparkle.Forum.Repository.DbContexts;

namespace TwilightSparkle.Forum.DatabaseSeed
{
    public static class DatabaseMigrationSeed
    {
        public static void SeedMigrateDatabase(DatabaseContext appContext)
        {
            appContext.Database.Migrate();
        }
    }
}