using System.Linq;

using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.DbContexts;

namespace TwilightSparkle.Forum.DatabaseSeed
{
    public static class DatabaseMigrationSeed
    {
        public static void SeedMigrateDatabase(DatabaseContext appContext)
        {
            appContext.Database.Migrate();
            var defaultProfileImage = appContext.Images.FirstOrDefault(i => i.ExternalId == "default-profile-image");
            if(defaultProfileImage == null)
            {
                defaultProfileImage = new UploadedImage
                {
                    ExternalId = "default-profile-image",
                    MediaType = "image/png",
                    FilePath = "default-profile-image.png"
                };
                appContext.Images.Add(defaultProfileImage);
                appContext.SaveChanges();
            }
        }
    }
}