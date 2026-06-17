using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    /// Represents a blog post entity.
    public class Blog
    {
        /// Gets or sets the unique identifier for the blog post.
        [Key]
        public int Id { get; set; }

        /// Gets or sets the title of the blog post.
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        /// Gets or sets the main body content of the blog post.
        [Required]
        public string Content { get; set; } = string.Empty;
        /// Gets or sets the priority weight of the blog post (e.g. 1 to 5).
        public int Priority { get; set; } = 1;
        /// Gets or sets the creation date and time of the blog post in UTC.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// Gets or sets the unique identifier of the user who authored this blog post.
        [Required]
        public string UserId { get; set; } = string.Empty;
        /// Gets or sets the author user entity.
        public ApplicationUser? User { get; set; }
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
