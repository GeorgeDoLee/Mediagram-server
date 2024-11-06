namespace allnews.Models.DTOs
{
    public class AddArticleDto
    {
        public required string Title { get; set; }
        public string? Photo { get; set; }
        public Dictionary<Guid, string> PublisherUrls { get; set; } = new();
    }
}
