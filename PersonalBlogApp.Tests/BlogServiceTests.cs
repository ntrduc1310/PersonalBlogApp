using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Data;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;
using Xunit;

namespace PersonalBlogApp.Tests
{
    public class BlogServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly BlogService _blogService;

        public BlogServiceTests()
        {
            // Cấu hình In-Memory Database cho EF Core
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _blogService = new BlogService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Blogs.AddRange(
                new Blog { Id = 1, Title = "Blog 1", Content = "Content 1", UserId = "UserA", Priority = 1, CreatedAt = DateTime.UtcNow },
                new Blog { Id = 2, Title = "Blog 2", Content = "Content 2", UserId = "UserA", Priority = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-1) },
                new Blog { Id = 3, Title = "Blog 3", Content = "Content 3", UserId = "UserB", Priority = 3, CreatedAt = DateTime.UtcNow.AddMinutes(-2) }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetBlogsAsync_AsAdmin_ReturnsAllBlogs()
        {
            // Arrange
            string userId = "UserA";
            bool isAdmin = true;

            // Act
            var result = await _blogService.GetBlogsAsync(userId, isAdmin, search: null, priority: null, sort: null, pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Equal(3, result.TotalItemCount);
        }

        [Fact]
        public async Task GetBlogsAsync_AsUserA_ReturnsOnlyOwnedBlogs()
        {
            // Arrange
            string userId = "UserA";
            bool isAdmin = false;

            // Act
            var result = await _blogService.GetBlogsAsync(userId, isAdmin, search: null, priority: null, sort: null, pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Equal(2, result.TotalItemCount);
        }

        [Fact]
        public async Task GetBlogsAsync_AsUserB_ReturnsOnlyOwnedBlogs()
        {
            // Arrange
            string userId = "UserB";
            bool isAdmin = false;

            // Act
            var result = await _blogService.GetBlogsAsync(userId, isAdmin, search: null, priority: null, sort: null, pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Equal(1, result.TotalItemCount);
        }

        [Fact]
        public async Task GetBlogsAsync_WithPriorityFilter_ReturnsFilteredBlogs()
        {
            // Arrange
            string userId = "UserA";
            bool isAdmin = true;
            int priorityFilter = 2;

            // Act
            var result = await _blogService.GetBlogsAsync(userId, isAdmin, search: null, priority: priorityFilter, sort: null, pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Equal(1, result.TotalItemCount);
        }

        [Fact]
        public async Task GetBlogsAsync_WithSortPriority_ReturnsSortedBlogs()
        {
            // Arrange
            string userId = "UserA";
            bool isAdmin = true;
            string sortOrder = "priority";

            // Act
            var result = await _blogService.GetBlogsAsync(userId, isAdmin, search: null, priority: null, sort: sortOrder, pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Equal(3, result.TotalItemCount);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
