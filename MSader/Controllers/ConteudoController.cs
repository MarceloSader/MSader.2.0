using Microsoft.AspNetCore.Mvc;
using MSader.Helpers;

namespace MSader.Controllers
{
    public class ConteudoController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConteudoController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Destra()
        {
            ViewBag.Menu = NavigationHelper.BuildMenuHtml("Conteúdo", "Destra", _httpContextAccessor);

            return View("Destra");
        }

        public IActionResult MedicinaVeterinaria()
        {
            return View("MedicinaVeterinaria");
        }

        public IActionResult Tecnologia()
        {
            return View("Tecnologia");
        }

        public IActionResult Animais()
        {
            return View("Animais");
        }
    }
}
