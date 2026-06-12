using System.ComponentModel.DataAnnotations;

namespace PersonalBlogApp.ViewModels
{
    public class BlogCreateVM
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề bài viết")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn mức độ ưu tiên")]
        [Range(1, 5, ErrorMessage = "Mức độ ưu tiên phải từ 1 đến 5")]
        [Display(Name = "Mức độ ưu tiên")]
        public int Priority { get; set; } = 1;
    }

    public class BlogEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tiêu đề không vượt quá 200 ký tự")]
        [Display(Name = "Tiêu đề bài viết")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn mức độ ưu tiên")]
        [Range(1, 5, ErrorMessage = "Mức độ ưu tiên phải từ 1 đến 5")]
        [Display(Name = "Mức độ ưu tiên")]
        public int Priority { get; set; } = 1;
    }
}