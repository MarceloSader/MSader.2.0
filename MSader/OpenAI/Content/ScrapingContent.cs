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
