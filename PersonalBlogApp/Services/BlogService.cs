using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Data;
using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync(string userId, bool isAdmin)
        {
            IQueryable<Blog> query = _context.Blogs.Include(b => b.User);

            // Role-based filtering logic handled in Service Layer
            if (!isAdmin)
            {
                query = query.Where(b => b.UserId == userId);
            }

            // Order by newest blogs first (Default sort)
            return await query.OrderByDescending(b => b.CreatedAt).ToListAsync();
        }
        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            // Dùng Include để lấy kèm thông tin Tác giả (User) hiển thị trên trang chi tiết
            return await _context.Blogs
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task CreateBlogAsync(BlogCreateVM model, string userId)
        {
            var blog = new Blog
            {
                Title = model.Title,
                Content = model.Content,
                Priority = model.Priority,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }
    }
}
