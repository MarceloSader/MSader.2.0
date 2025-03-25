using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Projeto()
        {
            /// teste 
            return View("Projeto");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}