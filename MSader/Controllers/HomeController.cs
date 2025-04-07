using Microsoft.AspNetCore.Mvc;
using MSader.Helpers;

namespace MSader.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.Menu = NavigationHelper.BuildMenuHtml("Home", "", _httpContextAccessor);

            return View("Index");
        }

        public IActionResult Jornada()
        {
            ViewBag.Menu = NavigationHelper.BuildMenuHtml("Jornada", "", _httpContextAccessor);

            return View("Jornada");
        }

        public IActionResult Tools()
        {
            ViewBag.Menu = NavigationHelper.BuildMenuHtml("Ferramentas", "", _httpContextAccessor);

            return View("Tools");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}