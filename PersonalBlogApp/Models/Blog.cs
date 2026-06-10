using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlogApp.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int Priority { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key to User
        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
