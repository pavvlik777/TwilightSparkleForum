using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.DbContexts;

namespace TwilightSparkle.Forum.DatabaseSeed
{
    public static class DatabaseMigrationSeed
    {
        private static IReadOnlyCollection<string> _sectionNames;


        static DatabaseMigrationSeed()
        {
            var sectionNames = new List<string>
            {
                "News",
                "Arts",
                "Politics",
                "Friendship"
            };
            _sectionNames = sectionNames;
        }


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

            foreach(var sectionName in _sectionNames)
            {
                var section = appContext.Sections.FirstOrDefault(s => s.Name == sectionName);
                if(section != null)
                {
                    continue;
                }

                section = new Section
                {
                    Name = sectionName
                };
                appContext.Sections.Add(section);
                appContext.SaveChanges();
            }
        }
    }
}