using Microsoft.AspNetCore.Mvc;
using MSader.Helpers;
using MSader.Models;
using System.Diagnostics;

namespace MSader.Controllers
{
    
    public class AIToolsController : Controller
    {
        string msgReturn = "";

        string stStatus = "";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<AIToolsController> _logger;

        public AIToolsController(ILogger<AIToolsController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;

            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult AINaturalLanguage()
        {
            ViewBag.Menu = NavigationHelper.BuildMenuHtml("Inteligência Artificial", "Destra", _httpContextAccessor);

            return View("AINaturalLanguage");
        }

        //public IActionResult GetTextFromSpeechAudio()
        //{
        //    string transcribed = "";

        //    long size = files.Sum(f => f.Length);

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var filePath = Path.GetTempFileName();

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    Process uploaded files
        //    Don't rely on or trust the FileName property without validation.

        //    return Ok(new { count = files.Count, size });
        //}
    }
}
