using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace allnews.Services
{
    public class ScrapedArticle
    {
        public required string Url { get; set; }
        public required string Title { get; set; }
    }
    public class ArticleScraper
    {
        private readonly HttpClient _httpClient;

        public ArticleScraper()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ScrapedArticle> ScrapeArticle(string url, string titleClass, string articleClass)
        {
            var response = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var titleNode = SelectNodeByClass(doc, titleClass);
            var articleNode = SelectNodeByClass(doc, articleClass);

            return new ScrapedArticle
            {
                Url = url,
                Title = titleNode?.InnerText.Trim(),
                //ArticleText = articleNode?.InnerText.Trim(),
            };
        }

        private HtmlNode SelectNodeByClass(HtmlDocument doc, string className)
        {
            var xpathSelector = $"//*[contains(concat(' ', normalize-space(@class), ' '), ' {className} ')]";
            var node = doc.DocumentNode.SelectSingleNode(xpathSelector);

            if (node == null)
            {
                xpathSelector = $"//*[starts-with(concat(' ', normalize-space(@class), ' '), ' {className}')]";
                node = doc.DocumentNode.SelectSingleNode(xpathSelector);
            }

            return node;
        }
    }
}
