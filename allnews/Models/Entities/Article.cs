namespace allnews.Models.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Photo { get; set; }
        public required int OppCoverage { get; set; }
        public required int CenterCoverage { get; set; }
        public required int GovCoverage { get; set; }
        public int SubArticleCount { get; set; }
        public List<SubArticle> SubArticles { get; set; } = new List<SubArticle>();
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsBlindSpot { get; set; } = false;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public int TrendingScore { get; set; } = 0;
    }
}
