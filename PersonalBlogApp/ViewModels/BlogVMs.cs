using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    /// <summary>
    /// View model used when creating a new blog post.
    /// </summary>
    public class BlogCreateVM
    {
        /// <summary>
        /// Gets or sets the title of the blog.
        /// </summary>
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề bài viết")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML/text content of the blog.
        /// </summary>
        [Required(ErrorMessage = "Nội dung không được để trống")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority weight of the blog (e.g. 1 to 5).
        /// </summary>
        [Required(ErrorMessage = "Vui lòng chọn mức độ ưu tiên")]
        [Range(1, 5, ErrorMessage = "Mức độ ưu tiên phải từ 1 đến 5")]
        [Display(Name = "Mức độ ưu tiên")]
        public int Priority { get; set; } = 1;
    }

    /// <summary>
    /// View model used when editing an existing blog post.
    /// </summary>
    public class BlogEditVM
    {
        /// <summary>
        /// Gets or sets the unique identifier of the blog to edit.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the blog.
        /// </summary>
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề bài viết")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTML/text content of the blog.
        /// </summary>
        [Required(ErrorMessage = "Nội dung không được để trống")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the priority weight of the blog (e.g. 1 to 5).
        /// </summary>
        [Required(ErrorMessage = "Vui lòng chọn mức độ ưu tiên")]
        [Range(1, 5, ErrorMessage = "Mức độ ưu tiên phải từ 1 đến 5")]
        [Display(Name = "Mức độ ưu tiên")]
        public int Priority { get; set; } = 1;
    }
}