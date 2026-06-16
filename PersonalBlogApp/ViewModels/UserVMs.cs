using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    /// <summary>
    /// View model representing a user entry item in the administration list.
    /// </summary>
    public class UserItemVM
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the role assigned to the user.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// View model used when editing an existing user's role and activation status.
    /// </summary>
    public class UserEditVM
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        [Required]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the role assigned to the user.
        /// </summary>
        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }
    }
}
