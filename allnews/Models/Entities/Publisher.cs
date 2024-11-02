namespace allnews.Models.Entities
{
    public class Publisher
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Logo { get; set; }
        public required string Position { get; set; }
        public required string TitleClass { get; set; }
        public required string ArticleClass { get; set; }
    }
}
