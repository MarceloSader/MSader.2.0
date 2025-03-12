using Microsoft.AspNetCore.Mvc;

namespace MSader.Controllers
{
    public class FotografiaController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
