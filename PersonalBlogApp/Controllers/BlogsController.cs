using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Filters;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Controllers
{
    [Authorize] // Bắt buộc đăng nhập
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        // GET: /Blogs?sort=priority
        public async Task<IActionResult> Index(string sort)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var blogs = await _blogService.GetBlogsAsync(currentUserId ?? string.Empty, isAdmin, sort);

            // Dùng ViewData để lưu giữ trạng thái sắp xếp hiện tại truyền ra View hiển thị active nút
            ViewData["CurrentSort"] = sort;
            return View(blogs);
        }

        // GET: /Blogs/Details/5
        //[AuthorizeBlogOwner] // Tự động chặn xem bài của người khác nếu không phải Admin/Chủ sở hữu
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            return View(blog);
        }

        // GET: /Blogs/Create
        public IActionResult Create()
        {
            return View(new BlogCreateVM());
        }

        // POST: /Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var currentUserId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(currentUserId)) return Challenge();

            await _blogService.CreateBlogAsync(model, currentUserId);

            TempData["SuccessMessage"] = "Đăng bài viết mới thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Blogs/Edit/5
        [AuthorizeBlogOwner] 
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            var model = new BlogEditVM
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Priority = blog.Priority
            };

            return View(model);
        }

        // POST: /Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeBlogOwner]
        public async Task<IActionResult> Edit(int id, BlogEditVM model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            await _blogService.UpdateBlogAsync(model);

            TempData["SuccessMessage"] = "Cập nhật bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Blogs/Delete/5
        [AuthorizeBlogOwner]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null) return NotFound();

            return View(blog);
        }

        // POST: /Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeBlogOwner]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteBlogAsync(id);

            TempData["SuccessMessage"] = "Xóa bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}