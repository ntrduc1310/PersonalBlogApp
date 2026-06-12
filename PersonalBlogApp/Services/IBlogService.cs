using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogsAsync(string userId, bool isAdmin, string sort = "");
        Task<Blog?> GetBlogByIdAsync(int id);

        // Đồng bộ kiểu dữ liệu đầu vào theo đúng ViewModel
        Task CreateBlogAsync(BlogCreateVM model, string userId);
        Task UpdateBlogAsync(BlogEditVM model);

        Task DeleteBlogAsync(int id);
        Task<bool> IsUserAuthorizedAsync(int id, string userId, bool isAdmin);
    }
}