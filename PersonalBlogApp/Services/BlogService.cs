using Microsoft.EntityFrameworkCore;
using PersonalBlogApp.Data;
using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;
using X.PagedList;

namespace PersonalBlogApp.Services
{
    /// Service layer handling all business logic and data access operations for Blogs and Comments.
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        /// Initializes a new instance of the BlogService.
        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// Retrieves all blogs from the database with pagination, optionally sorted by priority or creation date.
        /// Uses IQueryable for deferred query execution under the hood and applies Skip/Take at the database level.
        public async Task<IPagedList<Blog>> GetBlogsAsync(string userId, bool isAdmin, string? search, int? priority, string? sort, int pageNumber, int pageSize)
        {
            // Use IQueryable to defer database query execution and compile optimal SQL queries.
            IQueryable<Blog> query = _context.Blogs.Include(b => b.User);

            if (!isAdmin)
            {
                query = query.Where(b => b.UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchTerm = $"%{search}%";
                query = query.Where(b => EF.Functions.Like(b.Title, searchTerm) || EF.Functions.Like(b.Content, searchTerm));
            }

            if (priority.HasValue)
            {
                query = query.Where(b => b.Priority == priority.Value);
            }

            // Handle sorting requests: sort either by priority (descending) or creation date (newest first).
            if (sort == "priority")
            {
                query = query.OrderByDescending(b => b.Priority)
                             .ThenByDescending(b => b.CreatedAt);
            }
            else
            {
                query = query.OrderByDescending(b => b.CreatedAt);
            }
           // string generatedSql = query.ToQueryString();
            //=>SQLS
            return await query.ToPagedListAsync(pageNumber, pageSize);
            //automatic render OFFSET and LIMIT or SKIP and TAKE to handle 10 currently blogs on display page
            ////to change list co phan trang IpagedList<blog>
        }

        public async Task<Blog?> GetBlogByIdAsync(int id)//Tai all list datas lquan request duy nhat
        {
            return await _context.Blogs
                .Include(b => b.User)
                // Sắp xếp bình luận mới nhất lên đầu ngay trong Include 
                .Include(b => b.Comments.OrderByDescending(c => c.CreatedAt))
                    .ThenInclude(c => c.User) // Tải thông tin người bình luận
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Blog?> GetBlogByIdOnlyAsync(int id)
        {
            return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
        }

        // Ánh xạ dữ liệu từ ViewModel sang Model DB để tiến hành lưu mới
        public async Task<int> CreateBlogAsync(BlogCreateVM model, string userId)
        {
            var blog = new Blog
            {
                Title = model.Title,
                Content = model.Content,
                Priority = model.Priority,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            //hàng chờ thêm vào DB
            _context.Blogs.Add(blog);
            //luân chuyển vào DB
            await _context.SaveChangesAsync();

            return blog.Id;
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