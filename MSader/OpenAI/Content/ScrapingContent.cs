using msader.Helpers;
using NUnit.Framework;
using System.Net;
using HtmlAgilityPack;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using Azure.AI.OpenAI;
using Azure;

namespace MSader.OpenAI.Content
{

    public partial class ScrapingContent
    {
        [Test]
        public string GetContent(string url)
        {

            var web = new HtmlWeb();

            // connect to target page
            HtmlDocument document = web.Load(url);

            var root = document.DocumentNode;

            var sb = new StringBuilder();
            
            foreach (var node in root.DescendantsAndSelf())
            {
                if (!node.HasChildNodes)
                {
                    string text = node.InnerText;
                    if (!string.IsNullOrEmpty(text))
                        sb.AppendLine(text.Trim());
                }
            }

            return sb.ToString();
        }
    }


    public class WebScrapping
    {

    }

    public class Website
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36";
        public string Url { get; }
        public string Title { get; private set; }
        public string Text { get; private set; }
        public List<string> Links { get; private set; }

        public Website(string url)
        {
            Url = url;
        }

        public async Task FetchContentsAsync()
        {
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
            var response = await client.GetAsync(Url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch the URL: {Url}. Status code: {response.StatusCode}");
            }

            var body = await response.Content.ReadAsStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(body);

            // Extracting title and body text, excluding irrelevant HTML tags like scripts and images
            Title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText ?? "No title found";
            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            if (bodyNode != null)
            {
                foreach (var node in bodyNode.SelectNodes("//script|//style|//img|//input") ?? Enumerable.Empty<HtmlNode>())
                {
                    node.Remove();
                }
                Text = bodyNode.InnerText.Trim();
            }
            else
            {
                Text = string.Empty;
            }

            // Extracting all links from anchor tags
            Links = doc.DocumentNode.SelectNodes("//a[@href]")
                ?.Select(node => node.GetAttributeValue("href", string.Empty))
                .Where(href => !string.IsNullOrEmpty(href))
                .ToList() ?? new List<string>();
        }

        public string GetContents()
        {
            return $"Webpage Title:\n{Title}\nWebpage Contents:\n{Text}\n\n";
        }

        public List<string> GetLinks()
        {
            return Links;
        }
    }
}
