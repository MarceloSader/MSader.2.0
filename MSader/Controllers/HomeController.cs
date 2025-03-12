using Microsoft.AspNetCore.Mvc;
using MSader.Models;
using System.Diagnostics;

namespace MSader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            /// teste 
            return View("Index");
        }

        public IActionResult Jornada()
        {
            return View("Jornada");
        }

        public IActionResult Tools()
        {
            return View("Tools");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}