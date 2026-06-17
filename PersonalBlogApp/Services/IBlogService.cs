using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;
using X.PagedList;

namespace PersonalBlogApp.Services
{

    /// Service interface handling all operations for blogs and comments.
    /// </>
    public interface IBlogService
    {
        /// Retrieves all blogs from the database with pagination, optionally filtered or sorted.
        Task<IPagedList<Blog>> GetBlogsAsync(string userId, bool isAdmin, string? search, int? priority, string? sort, int pageNumber, int pageSize);

        /// Retrieves a single blog by ID, including its author details and associated comments
        Task<Blog?> GetBlogByIdAsync(int id);
        /// Creates and saves a new blog post to the database.
        Task CreateBlogAsync(BlogCreateVM model, string userId);
        /// Updates an existing blog post details in the database.
        Task UpdateBlogAsync(BlogEditVM model);
        /// Deletes a blog post from the database by ID.
        Task DeleteBlogAsync(int id);
        /// Checks if the user is authorized to perform changes on a specific blog post (must be owner or admin)
        Task<bool> IsUserAuthorizedAsync(int id, string userId, bool isAdmin);
        /// Adds a new comment to a blog post.
        Task<Comment> AddCommentAsync(CommentCreateVM model, string userId);
        /// Retrieves a comment by ID.
        Task<Comment?> GetCommentAsync(int id);
        /// Deletes a comment by ID.
        Task DeleteCommentAsync(int id);
        /// Checks if the user is authorized to delete a specific comment (must be owner or admin).
        Task<bool> IsCommentOwnerOrAdminAsync(int commentId, string userId, bool isAdmin);
    }
}