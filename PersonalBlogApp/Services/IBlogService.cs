using PersonalBlogApp.Models;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogsAsync(string userId, bool isAdmin);

        Task CreateBlogAsync(BlogCreateVM model, string userId);
        Task<Blog?> GetBlogByIdAsync(int id);

    }
}