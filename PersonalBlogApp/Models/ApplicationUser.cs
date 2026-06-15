using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength]
        public string? AvatarUrl { get; set; }

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
