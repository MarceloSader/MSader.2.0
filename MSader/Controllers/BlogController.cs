using Microsoft.AspNetCore.Mvc;
using MSader.BLL;
using MSader.DTO;

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
