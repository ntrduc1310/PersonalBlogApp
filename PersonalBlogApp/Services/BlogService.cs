using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Data;
using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels; 
namespace PersonalBlogApp.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the BlogService.
        /// </summary>
        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetBlogsAsync(string userId, bool isAdmin, string sort = "")
        {
            // Sử dụng IQueryable để tối ưu hiệu năng truy vấn dữ liệu (Deferred Execution)
            IQueryable<Blog> query = _context.Blogs.Include(b => b.User);


            // Xử lý logic sắp xếp (Sắp xếp theo độ ưu tiên giảm dần hoặc mới nhất)
            if (sort == "priority")
            {
                query = query.OrderByDescending(b => b.Priority)
                             .ThenByDescending(b => b.CreatedAt);
            }
            else
            {
                query = query.OrderByDescending(b => b.CreatedAt);
            }

            return await query.ToListAsync();
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)
        {
            return await _context.Blogs
                .Include(b => b.User)
                // Sắp xếp bình luận mới nhất lên đầu ngay trong Include (Tính năng của EF Core 5+)
                .Include(b => b.Comments.OrderByDescending(c => c.CreatedAt))
                    .ThenInclude(c => c.User) // Tải thông tin người bình luận
                .FirstOrDefaultAsync(b => b.Id == id);
        }


        // Ánh xạ dữ liệu từ ViewModel sang Model DB để tiến hành lưu mới
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

        // Lấy bài viết cũ ra và cập nhật thông tin theo dữ liệu chỉnh sửa của ViewModel
        public async Task UpdateBlogAsync(BlogEditVM model)
        {
            var blog = await _context.Blogs.FindAsync(model.Id);
            if (blog != null)
            {
                blog.Title = model.Title;
                blog.Content = model.Content;
                blog.Priority = model.Priority;

                // Entity Framework sẽ tự động tracking thay đổi và cập nhật xuống DB
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBlogAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
            }
        }

        // Triển khai hàm kiểm tra quyền: Phải là chủ bài viết hoặc Admin mới được Sửa/Xóa
        public async Task<bool> IsUserAuthorizedAsync(int id, string userId, bool isAdmin)
        {
            if (isAdmin) return true;

            var blogExists = await _context.Blogs
                .AnyAsync(b => b.Id == id && b.UserId == userId);

            return blogExists;
        }
        public async Task<Comment> AddCommentAsync(CommentCreateVM model, string userId)
        {
            var comment = new Comment
            {
                BlogId = model.BlogId,
                Content = model.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Load User reference to fetch user email/avatar information
            await _context.Entry(comment).Reference(c => c.User).LoadAsync();

            return comment;
        }

        public async Task<Comment?> GetCommentAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Blog)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsCommentOwnerOrAdminAsync(int commentId, string userId, bool isAdmin)
        {
            if (isAdmin) return true;

            var comment = await _context.Comments.FindAsync(commentId);
            return comment != null && comment.UserId == userId;
        }
    }
}