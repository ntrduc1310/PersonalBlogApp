using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    /// <summary>
    /// Represents a comment posted by a user under a blog post.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the comment.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comment creation date and time in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the identifier of the user who authored this comment.
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the author user entity.
        /// </summary>
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the blog post this comment belongs to.
        /// </summary>
        [Required]
        public int BlogId { get; set; }

        /// <summary>
        /// Gets or sets the associated blog post entity.
        /// </summary>
        public Blog? Blog { get; set; }
    }
}
