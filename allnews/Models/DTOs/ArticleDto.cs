namespace allnews.Models.DTOs
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Photo { get; set; }
        public List<Guid> SubArticleIds { get; set; } = new List<Guid>();
        public required double OppCoverage { get; set; }
        public required double CenterCoverage { get; set; }
        public required double GovCoverage { get; set; }
        public int SubArticleCount { get; set; }

    }
}
