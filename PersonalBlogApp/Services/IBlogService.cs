using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogsAsync(string userId, bool isAdmin, string sort = "");
        Task<Blog?> GetBlogByIdAsync(int id);
        Task CreateBlogAsync(BlogCreateVM model, string userId);
        Task UpdateBlogAsync(BlogEditVM model);

        Task DeleteBlogAsync(int id);
        Task<bool> IsUserAuthorizedAsync(int id, string userId, bool isAdmin);
        Task<Comment> AddCommentAsync(CommentCreateVM model, string userId);
        Task<Comment?> GetCommentAsync(int id);
        Task DeleteCommentAsync(int id);
        Task<bool> IsCommentOwnerOrAdminAsync(int commentId, string userId, bool isAdmin);
    }
}