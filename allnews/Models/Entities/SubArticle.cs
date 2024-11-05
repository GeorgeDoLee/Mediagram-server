namespace allnews.Models.Entities
{
    public class SubArticle
    {
        public Guid Id { get; set; }
        public required string Url { get; set; }
        public required string Title { get; set; }
        public required Guid PublisherId { get; set; }
    }
}
