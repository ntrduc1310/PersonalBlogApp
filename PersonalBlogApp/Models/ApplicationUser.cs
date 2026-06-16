using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.Models
{
    /// <summary>
    /// Custom identity user model extended from IdentityUser.
    /// Includes fields for custom profile avatar URL and active/deactive status.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the relative or absolute URL to the user's avatar image.
        /// </summary>
        [MaxLength]
        public string? AvatarUrl { get; set; }
<<<<<<< HEAD

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// Deactivated users cannot log into the system.
        /// </summary>
        public bool IsActive { get; set; } = true;
=======
>>>>>>> origin/master

        /// <summary>
        /// Gets or sets the collection of blog posts written by this user.
        /// </summary>
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

        /// <summary>
        /// Gets or sets the collection of comments posted by this user.
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
