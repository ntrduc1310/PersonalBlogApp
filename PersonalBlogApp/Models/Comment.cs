using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required]
        public int BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog? Blog { get; set; }
    }
}
