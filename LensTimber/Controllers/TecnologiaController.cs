using Microsoft.AspNetCore.Mvc;

namespace MSader.Controllers
{
    public class TecnologiaController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Features()
        {
            return View("Features");
        }

        public IActionResult MyTranscription()
        {
            string openAIResponse = "";

            //MSader.OpenAI.AudioFeatures audioFeatures = new AudioFeatures();
            //openAIResponse = audioFeatures.SimpleTranscription();

            return Json(new { openAIResponse });
        }

        public IActionResult VetCoders()
        {
            return View("VetCoders");
        }
    }
}
