using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using MSader.DTO;

namespace MSader.Helpers
{
    public class AIHelper   
    {
        public async Task<string> ObterRespostaDaAzureOpenAIAsync(string prompt)
        {
            try
            {
                var apiKey = ConstantsDTO.AZURE_OPEN_AI_API_KEY; //Environment.GetEnvironmentVariable("OPENAI_API_KEY");

                var endpoint = "https://openai-studio-dev.openai.azure.com/";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("api-key", apiKey);

                    var requestBody = new
                    {
                        messages = new[]
                        {
                        new { role = "system", content = "Você é um especialista em SQL Server." },
                        new { role = "user", content = prompt }
                    },
                        temperature = 0.7,
                        max_tokens = 1000
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
                throw ex;
            }

        }

        public async Task<string> ObterRespostaDaOpenAIAsync(string prompt, int nrMaxTokens, double vrTemperature)
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
                            new { role = "system", content = "Você é o editor do blog VetStories" },
                            new { role = "user", content = prompt }
                        },
                        max_tokens = nrMaxTokens,
                        temperature = 0.7
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
                throw ex;
            }


        }

    }
}
