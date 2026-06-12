using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace PersonalBlogApp.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admin can access this controller
    public class ManagementController : Controller
    {
        public IActionResult Users()
        {
            return Content("Admin Dashboard - User Management");
        }
    }
}
