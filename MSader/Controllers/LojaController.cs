using Microsoft.AspNetCore.Mvc;

namespace MSader.Controllers
{
    public class LojaController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
