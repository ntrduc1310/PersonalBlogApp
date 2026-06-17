using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    /// <summary>
    /// View model representing a user entry item in the administration list.

    public class UserItemVM
    {

        /// Gets or sets the user identifier.
        public string Id { get; set; } = string.Empty;

        /// Gets or sets the user email address.
        public string Email { get; set; } = string.Empty;

        /// Gets or sets the name of the role assigned to the user.
        public string Role { get; set; } = string.Empty;

        /// Gets or sets a value indicating whether the user account is active.  
        public bool IsActive { get; set; }
    }

    /// View model used when editing an existing user's role and activation status.
    public class UserEditVM
    {
        /// Gets or sets the user identifier.
        [Required]
        public string Id { get; set; } = string.Empty;

        /// Gets or sets the user email address.
        public string Email { get; set; } = string.Empty;

        /// Gets or sets the name of the role assigned to the user.
        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public string Role { get; set; } = string.Empty;

        /// Gets or sets a value indicating whether the user account is active.
        [Required]
        public bool IsActive { get; set; }
    }
}
