using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Services
{
    /// <summary>
    /// Service interface handling all operations for blogs and comments.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// Retrieves all blogs from the database, optionally filtered or sorted.
        /// </summary>
        Task<IEnumerable<Blog>> GetBlogsAsync(string userId, bool isAdmin, string sort = "");

        /// <summary>
        /// Retrieves a single blog by ID, including its author details and associated comments.
        /// </summary>
        Task<Blog?> GetBlogByIdAsync(int id);

        /// <summary>
        /// Creates and saves a new blog post to the database.
        /// </summary>
        Task CreateBlogAsync(BlogCreateVM model, string userId);

        /// <summary>
        /// Updates an existing blog post details in the database.
        /// </summary>
        Task UpdateBlogAsync(BlogEditVM model);

        /// <summary>
        /// Deletes a blog post from the database by ID.
        /// </summary>
        Task DeleteBlogAsync(int id);

        /// <summary>
        /// Checks if the user is authorized to perform changes on a specific blog post (must be owner or admin).
        /// </summary>
        Task<bool> IsUserAuthorizedAsync(int id, string userId, bool isAdmin);

        /// <summary>
        /// Adds a new comment to a blog post.
        /// </summary>
        Task<Comment> AddCommentAsync(CommentCreateVM model, string userId);

        /// <summary>
        /// Retrieves a comment by ID.
        /// </summary>
        Task<Comment?> GetCommentAsync(int id);

        /// <summary>
        /// Deletes a comment by ID.
        /// </summary>
        Task DeleteCommentAsync(int id);

        /// <summary>
        /// Checks if the user is authorized to delete a specific comment (must be owner or admin).
        /// </summary>
        Task<bool> IsCommentOwnerOrAdminAsync(int commentId, string userId, bool isAdmin);
    }
}