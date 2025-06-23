using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using LinkWise.Helpers;
using MSader.DTO;
using Microsoft.AspNetCore.Authorization;

namespace LinkWise.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContentController : Controller
    {
        #region Settings

        string msgReturn = "";

        string stStatus = "";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContentController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> RunContentGeneratorOpenAI(string dsp)
        {

            try
            {
                var prompt = new PromptPostGeneratorDTO(dsp);

                var oHelper = new AIHelper();

                // Chamada do método que envia o prompt para a OpenAI
                var resposta = await oHelper.GetRespostaDaOpenAIAsync(dsp);

                return Ok(new { res = resposta });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Ou use um logger
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetPostMetaData(string dsp)
        {

            try
            {

                var oHelper = new AIHelper();

                // Chamada do método que envia o prompt para a OpenAI
                var post = await oHelper.GetMetadataForPostAsync(dsp);

                return Ok(new { Post = post });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Ou use um logger
                return StatusCode(500, new { error = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> RunScraping(string url)
        {

            try
            {
                AIHelper scraper = new AIHelper();

                ScrapingDTO scraping = new ScrapingDTO();

                scraping.DSTextScraped = await scraper.GetMainContentFromUrlAsync(url);

                return Ok(new { scraping = scraping });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // Ou use um logger
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #region HELPERS

        private string GetUrlBase(IHttpContextAccessor httpContextAccessor)
        {
            var context = httpContextAccessor.HttpContext;

            if (context == null) return string.Empty;

            var request = context.Request;

            var urlBase = $"{request.Scheme}://{request.Host}";

            return urlBase;
        }

        // HELPERS
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

        #endregion
    }
}
