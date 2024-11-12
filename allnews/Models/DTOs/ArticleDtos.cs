using allnews.Models.Entities;

namespace allnews.Models.DTOs
{
    public class AddArticleDto
    {
        public required string Title { get; set; }
        public string? Photo { get; set; }
        public required Guid CategoryId { get; set; }
        public Dictionary<Guid, string> PublisherUrls { get; set; } = new();
    }

    public class ArticleSummaryDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Photo { get; set; }
        public int OppCoverage { get; set; }
        public int CenterCoverage { get; set; }
        public int GovCoverage { get; set; }
        public int SubArticleCount { get; set; }
        public bool IsBlindSpot { get; set; }
        public string CategoryName { get; set; }
    }
}
