using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Controllers
{
    [Authorize] // Task 4.1: Bắt buộc đăng nhập mới được Comment
    public class CommentsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Bình luận không hợp lệ hoặc quá dài.";
                return RedirectToAction("Details", "Blogs", new { id = model.BlogId });
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            await _blogService.AddCommentAsync(model, userId);

            TempData["SuccessMessage"] = "Đã thêm bình luận thành công!";

            // done=> đẩy người dùng quay lại đúng bài viết đó
            return RedirectToAction("Details", "Blogs", new { id = model.BlogId });
        }
    }
}