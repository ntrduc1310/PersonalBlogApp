using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Controllers
{
    /// Controller handling creation and deletion of blog comments.
    /// Access is restricted to authenticated users.
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        /// Initializes a new instance of the CommentsController.
        public CommentsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        /// Creates a new comment for a blog post.
        /// Supports AJAX requests (XMLHttpRequest) returning JSON, or falls back to traditional redirects.
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
                    avatarUrl = comment.User?.AvatarUrl,
                    message = "Thêm bình luận thành công!"
                });
            }

            TempData["SuccessMessage"] = "Đã thêm bình luận thành công!";

            // Done: Redirect the user back to the corresponding blog post
            return RedirectToAction("Details", "Blogs", new { id = model.BlogId });
        }


        /// Deletes a comment by ID if the current user is the owner or an admin.
        /// Supports AJAX requests returning JSON, or falls back to traditional redirects.
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
            
            // Server-side authorization check: Must be the comment owner or an Admin
            var isAuthorized = await _blogService.IsCommentOwnerOrAdminAsync(id, userId, isAdmin);
            if (!isAuthorized)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return StatusCode(403, new { success = false, message = "Không có quyền xóa bình luận này." });
                }
                // Return 403 Forbidden error (unauthorized)
                return Forbid();
            }
            var BlogId = comment.BlogId;
            
            await _blogService.DeleteCommentAsync(id);
            
            TempData["SuccessMessage"] = "Đã xóa bình luận thành công!";

            // AJAX response fallback
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Xóa bình luận thành công." });
            }
            return RedirectToAction("Details", "Blogs", new { id = BlogId });
        }   
    }
}