using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LinkWise.Helpers;
using MSader.DTO;
using Microsoft.Extensions.Hosting;

namespace LinkWise.Controllers
{
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
        public async Task<IActionResult> RunContentGeneratorOpenAI(int idp, int idtpr, string dst, string dsu, string nmer, string nmv, string nmt, string dso, string dscon, string dsp, string dscom, int nrm, double vrt)
        {

            try
            {
                var prompt = new PromptPostGeneratorDTO(idp, idtpr, dst, dsu, nmer, nmv, nmt, dso, dscon, dsp, dscom, nrm, vrt);

                // Aqui você pode montar o prompt final com base no DTO
                var promptMontado = MontarPromptPostGenerator(prompt);

                var oHelper = new AIHelper();

                // Chamada do método que envia o prompt para a OpenAI
                var resposta = await oHelper.ObterRespostaDaOpenAIAsync(promptMontado, prompt.NRMaxTokens, prompt.VRTemperature);

                var post = JsonSerializer.Deserialize<dynamic>(resposta, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(new { res = post });
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
