using Microsoft.AspNetCore.Mvc;


namespace MSader.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
