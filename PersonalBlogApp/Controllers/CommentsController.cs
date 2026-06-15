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
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return BadRequest(new { success = false, message = "Bình luận không hợp lệ hoặc quá dài." });
                }
                TempData["ErrorMessage"] = "Bình luận không hợp lệ hoặc quá dài.";
                return RedirectToAction("Details", "Blogs", new { id = model.BlogId });
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Unauthorized();
                }
                return Challenge();
            }

            var comment = await _blogService.AddCommentAsync(model, userId);

            // Hybrid Response Pattern: If the request is an AJAX call (detected by the X-Requested-With header),
            // return a JsonResult containing serialized comment details for dynamic UI rendering.
            // Otherwise, fall back to a full-page redirect.
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    success = true,
                    commentId = comment.Id,
                    content = comment.Content,
                    createdAt = comment.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    userEmail = comment.User?.Email ?? "Ẩn danh",
                    avatarInitial = (comment.User?.Email?.Substring(0, 1).ToUpper() ?? "U"),
                    message = "Thêm bình luận thành công!"
                });
            }

            TempData["SuccessMessage"] = "Đã thêm bình luận thành công!";

            // done=> đẩy người dùng quay lại đúng bài viết đó
            return RedirectToAction("Details", "Blogs", new { id = model.BlogId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _blogService.GetCommentAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Unauthorized();
                }
                return Challenge();
            }
            var isAdmin = User.IsInRole("Admin");
            
            // Kiểm tra quyền Server-side: Phải là chủ bình luận hoặc Admin
            var isAuthorized = await _blogService.IsCommentOwnerOrAdminAsync(id, userId, isAdmin);
            if (!isAuthorized)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return StatusCode(403, new { success = false, message = "Không có quyền xóa bình luận này." });
                }
                // Trả về lỗi 403 forbidden ( Không có quyền)
                return Forbid();
            }
            var BlogId = comment.BlogId;
            
            await _blogService.DeleteCommentAsync(id);
            
            TempData["SuccessMessage"] = "Đã xóa bình luận thành công!";

            // Neu request gui bang AJAX (IsAjaxRequest())
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Xóa bình luận thành công." });
            }

            return RedirectToAction("Details", "Blogs", new { id = BlogId });
        }   
    }
}