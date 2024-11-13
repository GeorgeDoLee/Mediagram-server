namespace Mediagram.Models.Entities
{
    public class SubArticle
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public required Guid PublisherId { get; set; }
        public required string Url { get; set; }
        public required string Title { get; set; }
        public Publisher? Publisher { get; set; }
    }
}
