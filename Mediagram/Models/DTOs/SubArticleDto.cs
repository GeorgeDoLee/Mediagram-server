namespace Mediagram.Models.DTOs
{
    public class SubArticleDto
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid PublisherId { get; set; }
        public required string Url { get; set; }
        public required string Title { get; set; }
    }
}
