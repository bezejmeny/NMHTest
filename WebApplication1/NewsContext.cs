using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using WebApplication1.Entities;

namespace WebApplication1
{
    public class NewsContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Site> Sites { get; set; }

        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Image>()
                .Property(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<Author>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Author>()
                .HasIndex(b => b.Name)
                .IsUnique();
            modelBuilder.Entity<Author>()
                .HasOne(b => b.Image)
                .WithOne()
                .HasForeignKey<Image>(b => b.Id)
                .IsRequired();//needed?
            modelBuilder.Entity<Article>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Article>()
                .Property(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<Article>()
                .HasIndex(b => b.Title);
            modelBuilder.Entity<Article>()
                .HasMany(b => b.Author)
                .WithMany();
            modelBuilder.Entity<Article>()
                .HasOne(b => b.Site)
                .WithMany()
                .HasForeignKey(b => b.Id)
                .IsRequired();
            modelBuilder.Entity<Site>()
                .HasKey(b => b.Id);
        }
    }
}
