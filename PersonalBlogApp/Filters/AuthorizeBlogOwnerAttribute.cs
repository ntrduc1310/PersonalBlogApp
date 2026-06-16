using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonalBlogApp.Models;
using PersonalBlogApp.Services;

namespace PersonalBlogApp.Filters
{
    /// <summary>
    /// Custom authorization attribute to restrict blog editing/deletion to the owner or admins.
    /// </summary>
    public class AuthorizeBlogOwnerAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the AuthorizeBlogOwnerAttribute.
        /// </summary>
        public AuthorizeBlogOwnerAttribute() : base(typeof(AuthorizeBlogOwnerFilter))
        {
        }

        /// <summary>
        /// Action filter implementing the authorization logic for checking blog ownership or admin rights.
        /// </summary>
        private class AuthorizeBlogOwnerFilter : IAsyncActionFilter
        {
            private readonly IBlogService _blogService;
            private readonly UserManager<ApplicationUser> _userManager;

            /// <summary>
            /// Initializes a new instance of the AuthorizeBlogOwnerFilter.
            /// </summary>
            public AuthorizeBlogOwnerFilter(IBlogService blogService, UserManager<ApplicationUser> userManager)
            {
                _blogService = blogService;
                _userManager = userManager;
            }

            /// <summary>
            /// Executes the filter verification logic asynchronously.
            /// </summary>
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                // Retrieve the "id" parameter from the Route URL (e.g. /Blogs/Edit/5)
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

                    // Invoke the service to check authorization
                    var isAuthorized = await _blogService.IsUserAuthorizedAsync(id, currentUserId, isAdmin);

                    if (!isAuthorized)
                    {
                        // Intercept the execution and return a 403 Forbidden result
                        context.Result = new ForbidResult();
                        return;
                    }
                }

                // User is authorized, allow the action to continue execution
                await next();
            }
        }
    }
}