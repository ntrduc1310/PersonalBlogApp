using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Models;

namespace PersonalBlogApp.Data
{
    /// <summary>
    /// Database context for the application, handling Identity tables, Blogs, and Comments.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        /// Initializes a new instance of the ApplicationDbContext with given options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        /// Gets or sets the database set for Blogs.
        public DbSet<Blog> Blogs { get; set; }
        /// Gets or sets the database set for Comments.
        public DbSet<Comment> Comments { get; set; }
        /// Configures database table relationships, foreign keys, and delete behaviors on model creation.
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
