using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using MSader.DTO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace LinkWise.Helpers
{
    public class AIHelper
    {

        #region Settings

        #region Settings

        private readonly List<string> _selectors = new List<string>
        {
            "//article",                                // Muito comum em blogs e notícias
            "//div[@id='content']",                     // ID genérico para conteúdo
            "//div[@class='content-inner']",            // Classe customizada
            "//div[@class='txtnews']",                  // Classe da notícia no site www.animalshealth.es/ 
            "//div[@class='main-content']",             // Classe muito usada
            "//div[@class='post-content']",             // Conteúdo de posts
            "//section[@id='main']",                    // Section principal
            "//div[@id='main-content']",                // ID específico
            "//div[contains(@class, 'article')]",       // Div que contém 'article' na classe
            "//div[contains(@class, 'entry-content')]", // Wordpress padrão
            "//div[contains(@class, 'post-body')]"      // Blogger padrão
        };

        #endregion

        #endregion

        public async Task<string> GetRespostaDaOpenAIAsync(string prompt)
        {
            var apiKey = ConstantsDTO.OPEN_AI_API_KEY;// Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            var endpoint = "https://api.openai.com/v1/chat/completions";

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    var requestBody = new
                    {
                        model = "gpt-4", // ou "gpt-3.5-turbo"
                        messages = new[]
                        {
                            new { role = "system", content = "Você é o editor do blog LinkWise" },
                            new { role = "user", content = prompt }
                        }
                    };

                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(endpoint, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);

                    return jsonResponse.choices[0].message.content.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<PostDTO> GetMetadataForPostAsync(string prompt)
        {
            var apiKey = ConstantsDTO.OPEN_AI_API_KEY;// Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            var endpoint = "https://api.openai.com/v1/chat/completions";

            var postDto = new PostDTO();

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    var requestBody = new
                    {
                        model = "gpt-4", // ou "gpt-3.5-turbo"
                        messages = new[]
                        {
                            new { role = "system", content = "Você é o editor do blog LinkWise" },
                            new { role = "user", content = prompt }
                        }
                    };

                    var json = JsonConvert.SerializeObject(requestBody);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(endpoint, content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);

                    string respostaIa = jsonResponse.choices[0].message.content;

                    postDto = JsonConvert.DeserializeObject<PostDTO>(respostaIa);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return postDto;

        }

        public async Task<string> GetMainContentFromUrlAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MyScraperBot/1.0)");

                    var html = await client.GetStringAsync(url);

                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    List<HtmlNode> candidateNodes = new List<HtmlNode>();

                    // Coleta todos os nós que batem nos seletores
                    foreach (var selector in _selectors)
                    {
                        var node = htmlDoc.DocumentNode.SelectSingleNode(selector);
                        if (node != null)
                        {
                            candidateNodes.Add(node);
                        }
                    }

                    // Se encontrou candidatos via seletores, escolhe o maior
                    HtmlNode bestNode = candidateNodes.OrderByDescending(n => CleanText(n.InnerText).Length).FirstOrDefault();

                    // Se nenhum seletor funcionou, aplica fallback no <body>
                    if (bestNode == null)
                    {
                        Console.WriteLine("Nenhum seletor específico encontrou conteúdo. Usando fallback para maior bloco de texto.");
                        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
                        bestNode = GetLargestTextBlock(bodyNode);
                    }

                    if (bestNode != null)
                    {
                        var cleaned = CleanText(bestNode.InnerText);
                        return string.IsNullOrWhiteSpace(cleaned) ? "Conteúdo não encontrado." : cleaned;
                    }

                    return "Conteúdo não encontrado.";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao acessar URL: {ex.Message}";
            }
        }


        private string CleanText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            text = System.Net.WebUtility.HtmlDecode(text);
            text = Regex.Replace(text, @"\r|\n|\t", " ");
            text = Regex.Replace(text, @"\s+", " ");
            return text.Trim();
        }

        private HtmlNode GetLargestTextBlock(HtmlNode bodyNode)
        {
            if (bodyNode == null)
                return null;

            var nodesWithText = bodyNode.Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Element && !string.IsNullOrWhiteSpace(n.InnerText))
                .Select(n => new
                {
                    Node = n,
                    TextLength = n.InnerText.Length
                })
                .OrderByDescending(n => n.TextLength);

            return nodesWithText.FirstOrDefault()?.Node;
        }
    }
}
