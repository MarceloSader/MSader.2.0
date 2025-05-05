using Microsoft.AspNetCore.Mvc;
using MSader.DTO;

namespace LinkWise.Controllers
{
    public class LabsController : Controller
    {
        public IActionResult AIGrowthEngine()
        {
            ViewBag.Menu = new MenuPublicDTO("Labs");

            return View("AIGrowthEngine");
        }

        public IActionResult CaniveteSuico()
        {
            ViewBag.Menu = new MenuPublicDTO("Labs");

            return View("CaniveteSuico");
        }
    }
}
