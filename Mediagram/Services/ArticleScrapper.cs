using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;

namespace Mediagram.Services
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

        public async Task<ScrapedArticle> ScrapeArticle(string url, string articleClass = "")
        {
            var response = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var title = GetTitleFromHeader(doc);
            title = HttpUtility.HtmlDecode(title)?.Trim();
            var articleNode = string.IsNullOrEmpty(articleClass) ? null : SelectNodeByClass(doc, articleClass);

            return new ScrapedArticle
            {
                Url = url,
                Title = title,
                // ArticleText = articleText?.Trim(),
            };
        }

        private string GetTitleFromHeader(HtmlDocument doc)
        {
            var titleNode = doc.DocumentNode.SelectSingleNode("//title");
            if (titleNode != null)
                return titleNode.InnerText;

            var metaOgTitle = doc.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
            if (metaOgTitle != null && metaOgTitle.Attributes["content"] != null)
                return metaOgTitle.Attributes["content"].Value;

            var metaTwitterTitle = doc.DocumentNode.SelectSingleNode("//meta[@name='twitter:title']");
            if (metaTwitterTitle != null && metaTwitterTitle.Attributes["content"] != null)
                return metaTwitterTitle.Attributes["content"].Value;

            var metaTitle = doc.DocumentNode.SelectSingleNode("//meta[@name='title']");
            if (metaTitle != null && metaTitle.Attributes["content"] != null)
                return metaTitle.Attributes["content"].Value;

            return null;
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

