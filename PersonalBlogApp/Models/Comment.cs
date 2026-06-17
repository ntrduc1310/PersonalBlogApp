using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    /// Represents a comment posted by a user under a blog post.
    public class Comment
    {
        /// Gets or sets the unique identifier for the comment.
        [Key]
        public int Id { get; set; }
        /// Gets or sets the content of the comment.
        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = string.Empty;
        /// Gets or sets the comment creation date and time in UTC.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// Gets or sets the identifier of the user who authored this comment.
        [Required]
        public string UserId { get; set; } = string.Empty;
        /// Gets or sets the author user entity.
        public ApplicationUser? User { get; set; }
        /// Gets or sets the identifier of the blog post this comment belongs to.
        [Required]
        public int BlogId { get; set; }
        /// Gets or sets the associated blog post entity.
        public Blog? Blog { get; set; }
    }
}
