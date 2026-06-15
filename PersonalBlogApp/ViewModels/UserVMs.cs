using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    public class UserItemVM
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class UserEditVM
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        public string Role { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
    }
}
