using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    /// <summary>
    /// View model used when posting a new comment.
    /// </summary>
    public class CommentCreateVM
    {
        /// <summary>
        /// Gets or sets the target blog post identifier.
        /// </summary>
        [Required]
        public int BlogId { get; set; }

        /// <summary>
        /// Gets or sets the text content of the comment.
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập nội dung bình luận.")]
        [MaxLength(500, ErrorMessage = "Bình luận không được vượt quá 500 ký tự.")]
        public string Content { get; set; } = string.Empty;
    }
}