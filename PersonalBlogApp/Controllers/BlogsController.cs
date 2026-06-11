using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;
using PersonalBlogApp.ViewModels;

namespace PersonalBlogApp.Controllers
{
    [Authorize] // Require authentication for all actions in this controller
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogsController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        // GET: /Blogs
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            // Fetch filtered data through Service Layer
            var blogs = await _blogService.GetBlogsAsync(currentUserId ?? string.Empty, isAdmin);
            return View(blogs);
        }

        // GET: /Blogs/Create
        public IActionResult Create()
        {
            return View(new BlogCreateVM());
        }
        // GET: /Blogs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogService.GetBlogByIdAsync(id);
            if (blog == null)
            {
                return NotFound(); 
            }
            return View(blog);
        }
        // POST: /Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Protect against CSRF attacks
        public async Task<IActionResult> Create(BlogCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUserId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Challenge();
            }

            // Save via Service Layer
            await _blogService.CreateBlogAsync(model, currentUserId);
            return RedirectToAction(nameof(Index));
        }
    }
}