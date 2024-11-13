
namespace Mediagram.Models.DTOs
{
    public class PublisherDto
    {
        public required string Name { get; set; }
        public string? Logo { get; set; }
        public required string Position { get; set; }
        public required string TitleClass { get; set; }
        public required string ArticleClass { get; set; }
    }
}
