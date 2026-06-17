using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    /// View model used when posting a new comment.
    public class CommentCreateVM
    {
        /// Gets or sets the target blog post identifier.
        [Required]
        public int BlogId { get; set; }

        /// Gets or sets the text content of the comment.
        [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận.")]
        [MaxLength(500, ErrorMessage = "Bình luận không được vượt quá 500 ký tự.")]
        public string Content { get; set; } = string.Empty;
    }
}