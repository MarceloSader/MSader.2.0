using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace MSader.Controllers
{
    
    public class AIToolsController : Controller
    {
        string msgReturn = "";
        string stStatus = "";

        public IActionResult AINaturalLanguage()
        {
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
