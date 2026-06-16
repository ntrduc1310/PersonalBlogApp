using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Required]
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
