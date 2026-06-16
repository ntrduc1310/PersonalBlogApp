using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    /// <summary>
    /// Represents a blog post entity.
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// Gets or sets the unique identifier for the blog post.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the blog post.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the main body content of the blog post.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority weight of the blog post (e.g. 1 to 5).
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// Gets or sets the creation date and time of the blog post in UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the unique identifier of the user who authored this blog post.
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the author user entity.
        /// </summary>
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Gets or sets the collection of comments written under this blog post.
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
