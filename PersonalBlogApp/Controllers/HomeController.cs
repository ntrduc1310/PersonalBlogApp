using Microsoft.AspNetCore.Mvc;
using PersonalBlogApp.Models;
using System.Diagnostics;

namespace PersonalBlogApp.Controllers
{
    /// <summary>
    /// Controller handling general application landing pages and basic error rendering.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the HomeController.
        /// </summary>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: /
        /// Renders the home landing page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Privacy
        /// Renders the privacy policy page.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Error
        /// Renders the generic application error page.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
