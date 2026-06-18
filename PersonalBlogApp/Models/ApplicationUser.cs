using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    /// <summary>
    /// Custom identity user model extended from IdentityUser.
    /// Includes fields for custom profile avatar URL and active/deactive status.
   
    public class ApplicationUser : IdentityUser
    {

        /// Gets or sets the relative or absolute URL to the user's avatar image
        [MaxLength]
        public string? AvatarUrl { get; set; }

        /// Gets or sets a value indicating whether the user account is active.
        /// Deactivated users cannot log into the system
        public bool IsActive { get; set; } = true;

        ///Gets or sets the collection of blog posts written by this user.
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

        /// Gets or sets the collection of comments posted by this user
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
