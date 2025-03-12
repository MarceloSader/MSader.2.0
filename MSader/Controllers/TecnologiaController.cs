using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using MSader.OpenAI.Audio;
using MSader.OpenAI.Content;

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

            AudioFeatures audioFeatures = new AudioFeatures();

            openAIResponse = audioFeatures.SimpleTranscription();

            return Json(new { openAIResponse });
        }

        public IActionResult GetContent()
        {

            ScrapingContent scrap = new ScrapingContent();

            string downloadString = scrap.GetContent("https://www.avma.org/news/cdc-report-avian-influenza-found-two-cats-linked-dairy-workers");

            return Json(new { downloadString });
        }

        public IActionResult VetCoders()
        {
            return View("VetCoders");
        }


        public async Task<ActionResult> ExtractText()
        {
            string url = "https://www.avma.org/news/cdc-report-avian-influenza-found-two-cats-linked-dairy-workers";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Extract all text
            var text = htmlDoc.DocumentNode.InnerText;

            return Content(text);
        }
    }
}
