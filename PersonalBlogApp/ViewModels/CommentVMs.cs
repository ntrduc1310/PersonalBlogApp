using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    public class CommentCreateVM
    {
        [Required]
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận.")]
        [MaxLength(500, ErrorMessage = "Bình luận không được vượt quá 500 ký tự.")]
        public string Content { get; set; } = string.Empty;
    }
}