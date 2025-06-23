using Microsoft.AspNetCore.Mvc;
using MSader.BLL;
using MSader.DTO;
using MSader.Helpers;
using System.Text.Json;
using System.Text;
using Microsoft.Testing.Platform.Extensions.Messages;
using System.Security.Cryptography;

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

            ViewBag.EstilosResposta = ListHelper.GetListEstilosResposta();

            ViewBag.TiposPost = ListHelper.GetListTipoPost();

            ViewBag.Pessoas = ListHelper.GetListPessoas();

            //ViewBag.Vieses = ListHelper.GetListVieses();

            ViewBag.Prompts = ListHelper.GetListPrompts(ConstDTO.TipoPrompt.NaturalLanguage.ID);

            return View("AINaturalLanguage");
        }

        // CONTENT GENERATOR

        [HttpPost]
        public IActionResult GetPrompt(int idp)
        {

            PromptPostGeneratorDTO prompt = new PromptPostGeneratorDTO();

            using (PromptBLL oBLL = new PromptBLL())
            {
                prompt = oBLL.GetPromptRequest(idp);
            }

            return Json(prompt);
        }

        [HttpPost]
        public async Task<IActionResult> RunContentGeneratorOpenAI(int idp, int idtpr, string dst, string dsu, string nmer, string nmv, string nmt, string dso, string dscon, string dsp, string dscom, int nrm, double vrt)
        {
            PostDTO? post = new PostDTO();

            PromptPostGeneratorDTO prompt = new PromptPostGeneratorDTO(idp, idtpr, dst, dsu, nmer, nmv, nmt, dso, dscon, dsp, dscom, nrm, vrt);

            // Aqui você pode montar o prompt final com base no DTO
            var promptMontado = MontarPromptPostGenerator(prompt);

            var oHelper = new AIHelper();

            // Chamada do método que envia o prompt para a OpenAI
            var resposta = await oHelper.ObterRespostaDaOpenAIAsync(promptMontado, prompt.NRMaxTokens, prompt.VRTemperature);

            post = JsonSerializer.Deserialize<PostDTO>(resposta);

            return Json(new { res = post });
        }

        [HttpPost]
        public IActionResult SavePrompt(int idp, int idt, int ide, int idv, string nmt, string dso, string dsc, string dsp, int nrm, double vrt)
        {
            PromptPostGeneratorDTO prompt = new PromptPostGeneratorDTO(idp, idt, ide, idv, nmt, dso, dsc, dsp, nrm, vrt);

            using (PromptBLL oBLL = new PromptBLL())
            {
                oBLL.SavePromptRequest(prompt);
            }
            return Json(new { st = "OK" });
        }

        private string MontarPromptPostGenerator(PromptPostGeneratorDTO prompt)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Contexto: {prompt.DSContexto}");
            sb.AppendLine($"Tema/Assunto: {prompt.DSTema}");
            sb.AppendLine($"Fonte: {prompt.DSUrlSource}");
            sb.AppendLine($"Objetivo: {prompt.DSObjetivo}");
            sb.AppendLine("Tarefa: ");
            sb.AppendLine($"{prompt.DSPrompt}");
            sb.AppendLine("");
            sb.AppendLine($"Observações Complementares: {prompt.DSComplemento}");
            sb.AppendLine("");
            sb.AppendLine("Formato de saída:");
            sb.AppendLine("Gerar um único json estruturado da seguinte maneira:");
            sb.AppendLine("");
            sb.AppendLine("{");
            sb.AppendLine("\"DSAncoraPost\": \"\", ");
            sb.AppendLine("\"DSTituloPost\": \"\", ");
            sb.AppendLine("\"DSSubTituloPost\": \"\", ");
            sb.AppendLine("\"DSTextoPost\": \"\", ");
            sb.AppendLine("\"DSTags\": \"\"");
            sb.AppendLine("}");

            return sb.ToString();
        }
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

