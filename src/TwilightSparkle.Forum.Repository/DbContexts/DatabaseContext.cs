using Microsoft.EntityFrameworkCore;

using TwilightSparkle.Forum.DomainModel.Entities;

namespace TwilightSparkle.Forum.Repository.DbContexts
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UploadedImage> Images { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Thread> Threads { get; set; }

        public DbSet<LikeDislike> Likes { get; set; }

        public DbSet<Commentary> Commentaries { get; set; }


        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Thread>().HasMany(t => t.Likes).WithOne(l => l.Thread).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.Likes).WithOne(l => l.User).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Thread>().HasMany(t => t.Commentaries).WithOne(c => c.Thread).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().HasMany(u => u.Commentaries).WithOne(c => c.Author).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasMany(u => u.Threads).WithOne(t => t.Author).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}