namespace allnews.Models.DTOs
{
    public class SubArticleDto
    {
        public Guid Id { get; set; }
        public required string Url { get; set; }
        public required string Title { get; set; }
        public Guid PublisherId { get; set; }
    }
}
