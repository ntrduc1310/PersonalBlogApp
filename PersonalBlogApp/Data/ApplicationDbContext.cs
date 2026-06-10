using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Comment relationships
            builder.Entity<Comment>()
                .HasOne(c => c.Blog)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid multiple cascade paths

            // Configure Blog relationships
            builder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
