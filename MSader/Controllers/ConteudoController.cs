using Microsoft.AspNetCore.Mvc;

namespace MSader.Controllers
{
    public class ConteudoController : Controller
    {
        public IActionResult Destra()
        {
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
