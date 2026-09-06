using MCV_Mini_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MCV_Mini_Project.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<AboutPlatform> AboutPlatforms { get; set; }
        public DbSet<AboutVision> AboutVisions { get; set; }
        public DbSet<CourseImage> CourseImages { get; set; }
        public DbSet<CourseInfo> CourseInfos { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Video> Videos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(120);
            });
        }
    }
}
