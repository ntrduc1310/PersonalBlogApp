using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using System.Diagnostics;

namespace PersonalBlogApp.Controllers
{

    /// Controller handling general application landing pages and basic error rendering.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// Initializes a new instance of the HomeController.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// GET: /
        /// Renders the home landing page.
        public IActionResult Index()
        {
            return View();
        }

        /// GET: /Home/Privacy
        /// Renders the privacy policy page.
        public IActionResult Privacy()
        {
            return View();
        }

        /// GET: /Home/Error
        /// Renders the generic application error page.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
