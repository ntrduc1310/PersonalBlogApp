using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;

namespace PersonalBlogApp.Filters
{
    public class AuthorizeBlogOwnerAttribute : TypeFilterAttribute
    {
        public AuthorizeBlogOwnerAttribute() : base(typeof(AuthorizeBlogOwnerFilter))
        {
        }

        private class AuthorizeBlogOwnerFilter : IAsyncActionFilter
        {
            private readonly IBlogService _blogService;
            private readonly UserManager<ApplicationUser> _userManager;

            public AuthorizeBlogOwnerFilter(IBlogService blogService, UserManager<ApplicationUser> userManager)
            {
                _blogService = blogService;
                _userManager = userManager;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                // Lấy tham số "id" từ Route URL (ví dụ: /Blogs/Edit/5)
                if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
                {
                    var user = context.HttpContext.User;
                    var currentUserId = _userManager.GetUserId(user);
                    var isAdmin = user.IsInRole("Admin");

                    if (string.IsNullOrEmpty(currentUserId))
                    {
                        context.Result = new ChallengeResult();
                        return;
                    }

                    // Gọi Service kiểm tra quyền
                    var isAuthorized = await _blogService.IsUserAuthorizedAsync(id, currentUserId, isAdmin);

                    if (!isAuthorized)
                    {
                        // Chặn đứng và ném về trang 403 Forbidden
                        context.Result = new ForbidResult();
                        return;
                    }
                }

                await next(); // Quyền hợp lệ, cho phép tiếp tục chạy action
            }
        }
    }
}